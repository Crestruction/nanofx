# NanoFX
一个轻量级的静态按钮站生成命令行工具  
项目开发：Crestruction/椎名菜羽项目组

项目使用F#语言，基于 .NET 5.0 开发，Windows需要安装 .NET 5 Runtime。Linux服务器需要使用CI/CD可以参考微软官方.NET安装配置文档。

## 创建工程
```ps
nanofx create -o ./project_path
```
`project_path` 为项目模板生成位置

## 生成静态站
```ps
nanofx build -c config_path -o output_path
```
`config_path` 为配置文件路径  
`output_path` 为项目生成路径

## 配置文件
```yaml
#相对于配置文件的根路径
base: ./
#网站基本信息配置
site:
  #网站名称，影响页头页脚的网站名和Title
  name: NanoFX Example
  #网页图标即favicon，必须为ico
  favicon: res/icon.ico
  #Header栏上的图标，必须为png
  headericon: res/icon.png
  #页脚上的版权信息
  copyright: Copyright ©2021 Crestruction.
#项目资源配置，这里引入了MaterialDesignLite作为演示
resources:
  #音频资源配置，按钮组根据以下数组进行排序
  audiosources: 
      #ID将指定一些元素的CSS Class
      #button的class会生成为nanofx-button-audio-nya
      #div上的class属性等也会受此影响，可以通过文本编辑器或F12调试工具查看
    - id: audio-nya
      #按钮组的名称，显示为Header
      title: NyaNya
      #音频资源根目录，按钮上的文字将会根据文件名填写
      path: res/audio/Nya
    - id: audio-anti
      title: 大Anti
      path: res/audio/Anti
  #额外的css类，将会根据数组顺序填写到HTML
  stylesheets: 
    - res/css/material.min.css
    - res/css/nanofx.css
  #额外的js，将会根据数组顺序填写到HTML
  javascripts: 
    - res/js/material.min.js
#额外样式class，这里引入了MaterialDesignLite作为演示
styles:
  #按钮组上的额外css class
  section:
    - my-nanofx-block 
    - mdl-card 
    - mdl-shadow--2dp
  #按钮组标题上的额外css class
  header: 
    - my-nanofx-header
  #按钮上的额外css class
  button: 
    - mdl-button 
    - mdl-js-button 
    - mdl-button--raised 
    - mdl-js-ripple-effect 
    - my-nanofx-button

```