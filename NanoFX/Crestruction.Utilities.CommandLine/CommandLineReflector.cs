using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.CommandLineUtils;

namespace Crestruction.Utilities.CommandLine
{
    /// <summary>
    /// Contains a converter that can reflect an assembly to Specified <see cref="CommandLineApplication"/>.
    /// </summary>
    public static class CommandLineReflector
    {
        /// <summary>
        /// Reflect an assembly and get all <see cref="CommandTask"/>s to the specified <see cref="CommandLineApplication"/>.
        /// </summary>
        /// <param name="app">The <see cref="CommandLineApplication"/> to receive configs.</param>
        /// <param name="assembly">The Assembly to reflect from.</param>
        public static void ReflectFrom(this CommandLineApplication app, Assembly assembly)
        {
            foreach (var ct in assembly.GetTypes()
                .Where(x => typeof(CommandTask).IsAssignableFrom(x)))
            {
                var command_attribute = ct.GetCustomAttribute<CommandAttribute>();
                if (command_attribute == null)
                {
                    continue;
                }
                app.Command(command_attribute.Name, command =>
                {
                    ConfigCommand(command, command_attribute.Description, ct);
                });
            }
        }

        /// <summary>
        /// Convert a <see cref="CommandTask"/> to <see cref="CommandLineApplication"/> configs.
        /// </summary>
        private static void ConfigCommand(CommandLineApplication command, string commandDescription, Type taskType)
        {
            // Config basic info.
            command.Description = commandDescription;
            command.HelpOption("-?|-h|--help");

            // Store argument list and option list.
            // so that when the command executed, all properties can be initialized from command lines.
            var argument_property_list = new List<(CommandArgument argument, PropertyInfo property)>();
            var option_property_list = new List<(CommandOption option, PropertyInfo property)>();

            // Enumerate command task properties to get enough metadata to config command.
            foreach (var property in taskType.GetTypeInfo().DeclaredProperties)
            {
                // Try to get argument and option info.
                var argument_attribute = property.GetCustomAttribute<CommandArgumentAttribute>();
                var option_attribute = property.GetCustomAttribute<CommandOptionAttribute>();

                if (argument_attribute != null && property.CanWrite)
                {
                    // Try to record argument info.
                    var argument = command.Argument(
                        argument_attribute.Name,
                        argument_attribute.Description);
                    argument_property_list.Add((argument, property));
                }
                else if (option_attribute != null && property.CanWrite)
                {
                    // Try to record option info.
                    CommandOptionType command_option_type;
                    if (typeof(IEnumerable<string>).IsAssignableFrom(property.PropertyType))
                    {
                        // If this property is a List<string>.
                        command_option_type = CommandOptionType.MultipleValue;
                    }
                    else if (typeof(string).IsAssignableFrom(property.PropertyType))
                    {
                        // If this property is a string.
                        command_option_type = CommandOptionType.SingleValue;
                    }
                    else if (typeof(bool).IsAssignableFrom(property.PropertyType))
                    {
                        // If this property is a bool.
                        command_option_type = CommandOptionType.NoValue;
                    }
                    else
                    {
                        continue;
                    }
                    var option = command.Option(
                        option_attribute.Template,
                        option_attribute.Description,
                        command_option_type);
                    option_property_list.Add((option, property));
                }
            }

            // Config how to execute the command.
            command.OnExecute(() =>
            {
                // Create a new instance of CommandTask to call the Run method.
                var command_task = (CommandTask)Activator.CreateInstance(taskType);

                // Initialize the instance with prepared arguments and options.
                foreach (var (argument, property) in argument_property_list)
                {
                    property.SetValue(command_task, argument.Value);
                }
                foreach (var (option, property) in option_property_list)
                {
                    switch (option.OptionType)
                    {
                        case CommandOptionType.MultipleValue:
                            property.SetValue(command_task, option.Values.ToList());
                            break;
                        case CommandOptionType.SingleValue:
                            property.SetValue(command_task, option.Value());
                            break;
                        case CommandOptionType.NoValue:
                            property.SetValue(command_task, option.HasValue());
                            break;
                        default:
                            continue;
                    }
                }

                // Call the Run method.
                return command_task?.Run() ?? -1;
            });
        }
    }
}