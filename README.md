# UABS
UABS (Unity Asset Bundle Seeker或者‘Unity资源包查找工具’) 是一款专门应用于Unity引擎的辅助模组工具。目前市面上已经热门的工具像是有
[UABEA](https://github.com/nesrak1/UABEA), [AssetStudio](https://github.com/Perfare/AssetStudio),
[AssetRipper](https://github.com/AssetRipper/AssetRipper) 等等。它们都有各自的特点，该工具也是如此。
在这个项目里，我的目的是要让文件索取变得更方便快捷以及缩短模组的制作流程。其次我个人也是模组制作者，你可以在[B站](https://space.bilibili.com/31525265)找到我。我会不定期发布一些制作模组的原创教程。


<p align="center">
    <img src="/readme_img/logo.png" width="300"/>
</p>

## 工具特点
1. 和AssetStudio一样可以预先查看图像文件，而且不需要事先加载 - 只有当你需要的时候才会读取文件。
2. 软件内的可管理缓存系统。仅需下载一次即可为部分功能大幅度增速。
3. 比其他工具更注重文件的查询。
4. 你从未见过的进阶模组教程，尽在我的[B站空间](https://space.bilibili.com/31525265)。

## 功能
该工具目前还处在建设阶段，所以功能尚不完全。

1. 显示图像材质，听取音频文件等（效果同AssetStudio）- 目前可以显示图像材质
2. 导出图像材质，音频文件等（效果同AssetStudio）- 目前可以导出图像材质
3. 改写材质文件（效果同UABEA）
4. 寻找资源包的依赖项并快速引导 - 目前可以找到依赖项但是不能跳转
5. 标记，备注资源文件
6. 快速寻找资源包中的文件
7. 尽可能把我在B站发布过的小工具加进来并实现自动化

## 使用库
[UABEA](https://github.com/nesrak1/UABEA) (MIT) - AssetsTools.Net & AssetsTools.Net.Extra 以及导出文件Json化。很多UI也都是参考的UABEA。

[AddressablesTools](https://github.com/nesrak1/AddressablesTools/releases) (MIT) - 做模组必要工具。

[BCnEncoder.NET](https://github.com/Nominom/BCnEncoder.NET) (MIT) - 处理部分棘手的图像格式。

[Newtonsoft.Json-for-Unity](https://github.com/applejag/Newtonsoft.Json-for-Unity) (MIT) - 最好的Json代码库。

[Noto Sans Simplified Chinese](https://fonts.google.com/noto/specimen/Noto+Sans+SC/license?lang=zh_Hans) (SIL Open Font License, Version 1.1)  - 中文字体。


## 安装
敬请期待


## 问题
该工具使用Unity引擎建造。我知道这会带来很多问题不过也有些显而易见的好处。

---

问题一：Unity有很多个版本，怎么知道该工具可以适用于其他Unity版本的游戏？

答：我认为游戏的版本不是很大的问题，该工具很多地方都是参考了UABEA，如果UABEA都没有问题那理论上来说该工具也没有问题。如果你遇到了与版本相关的问题，可以发issue我可以帮忙看看。
（该工具的建造版本为Unity 2021.3.33f1，你如果下载的话请确保是用的这个Unity版本）

---

问题二：你会把这个工具做成一个独立软件吗？

答：可能性不高。有些代码可能在编辑器里可以运作但是被做成软件就不能跑了，而且如果是认真做模组的话下载Unity还是很有必要的。

---

问题三：我看见你发的内容都是讲2D的，有3D的教程吗？

答：不好意思，3D个人没什么研究，只能说有空的话会看看。如果你有想做的游戏可以在issue发问。

---

## 特注
1. 工具的logo由本人绘制，使用字体是[HE'S DEAD Jim](https://www.dafont.com/hes-dead-jim.font)。顺便一提我很爱看星际迷航系列。
2. 如果你要二次发布该工具的话请标注一下作者（我）- Kolyn090，或者附上这篇repo的链接。非常感谢你的支持！
