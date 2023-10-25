# myKnight
 Knight Dream是我的毕业设计，该项目参考了许多UE插件源码和unity博主的项目，如果是想看《艾莉冒险》的话，因为项目属于工作室并且处于停止开发（就是砍了）阶段，在协议上不能公开使用，但艾莉冒险跟本项目使用相似的设计逻辑，UI方面为本项目的优化版增加了Dotween的动画，动作部分采用了playerController项目中的设计，相当于平替。  
 
 该项目是一款第三人称俯视角ARPG游戏，该项目核心设计有角色、怪物控制、任务、对话、背包系统以及传送和存档功能  
 
 该项目体量不大，大致分为两个核心部分  
 1、角色、怪物、场景交互设计  
 2、UI逻辑与数据交互
 
本项目和之后的一款3DARPG游戏《艾莉冒险》都使用了ScriptObject来作为数据载体
![image](https://github.com/ArashiHF/myKnight/assets/56665189/cf6d0118-aaa7-4395-b4b0-df0207a4f292)  
本项目绝大多数对象都使用该存储基类（大致如下，图3中还使用了自己编写的简易对话编辑器以及任务系统中需求的数据）  
![image](https://github.com/ArashiHF/myKnight/assets/56665189/e12996cc-c453-4253-ae48-91218521aec9)
![image](https://github.com/ArashiHF/myKnight/assets/56665189/596e7a0a-afaa-419f-9e48-6957f8087f82)
![image](https://github.com/ArashiHF/myKnight/assets/56665189/f1d4be1e-d830-43f4-8015-a0a99d989ec2)

#角色、怪物  
![image](https://github.com/ArashiHF/myKnight/assets/56665189/7925403b-be3e-43c0-8907-7f57bc13acfb)  
玩家和怪物来自同一起源，继承同一状态代码（也就是血量，等级等数据以及攻击受伤等），在这之下增加角色的控制代码以及怪物的AI系统（以下为怪物AI系统）  
![image](https://github.com/ArashiHF/myKnight/assets/56665189/57ff4172-dbf1-4024-9c6b-33dee6499621)  
移动则使用自动寻路插件NavMeshAgent，要剔除掉不可移动的位置采用获取hasPath这一数据来判断是否有路径，解法参考[这里](https://blog.csdn.net/qq_52855744/article/details/118724620)，还有一种根据动画适配移动的方法，该方法用在了艾莉冒险上.  
![image](https://github.com/ArashiHF/myKnight/assets/56665189/7e2be4c2-1bc2-4eb1-aad4-8b97eaa6baa5)  
角色动画机是比较麻烦的部分，采用动画分层，就是将不同的状态分开来做。  
![image](https://github.com/ArashiHF/myKnight/assets/56665189/829b732d-93e3-4df6-a9a9-2f37eeb1beea)  
两个项目都采用相似的设计，在玩家执行不同动作的时候切换动画状态，这里要特别讲一下，攻击动画在骑士的梦中只有两种攻击分别为普攻和暴击，但在艾莉冒险中采用跟怪物猎人相近的攻击派生，核心理念就是输入不同的指令会向着不同的动画走，大致如下  
![image](https://github.com/ArashiHF/myKnight/assets/56665189/3dbe5c78-ad8a-46c0-97c9-17e39d51c480)  
该设计只有两个指令，派生比较少但是也足够使用在最后转型成手游时也比较方便修改，该项目无缝衔接到手游。  

#UI逻辑与数据交互  
![image](https://github.com/ArashiHF/myKnight/assets/56665189/4185c406-dc71-4dfe-b5f1-27a36f0b0959)  
背包、对话、任务系统采用MVC模式，将对象分为多个部分，这部分主要由ScriptObject来承担数据存储，重要的部分是将整个背包分为多个背包，数据、交互与对应的控制器会放在不同的脚本来运作，减少耦合性，比较关键的地方在于使用List存储不同的数据类，将相同的对象归在特定的地方（类型如下）。  
![image](https://github.com/ArashiHF/myKnight/assets/56665189/4d0a0851-e53c-4e78-91eb-e9589517463c)  
背包系统中的换装系统是比较重要的部分，在装备预制体上装上对应的数据文件，涵盖动作、数值、装备模型、特效、声音等，即可在更换装备的时候也同步换上对应的数据，在艾莉冒险中为了减少开发的膨胀采用了获得的武器就是完全体，减去了所谓的技能树或者数值等，但在后面的手游化也计划添加上（如图）。  
![image](https://github.com/ArashiHF/myKnight/assets/56665189/6d8da669-2eca-44d2-87da-8e345623bc5d)

#存档与传送门系统  
![image](https://github.com/ArashiHF/myKnight/assets/56665189/d8b698cf-63cb-4926-9c5b-54da03bb1225)  
存档方面采用的是Json，将玩家的数据如等级、血量打包，背包、任务情况作为列表进行打包  
传送门系统主要在场景传送需要注意，采用的是异步加载，通过获取目标名字使用协程加载资源，实现方法参考[这里](https://blog.csdn.net/xinzhilinger/article/details/110836837)。  

总结：该项目主要为功能较为齐全，是本人用于找工作的毕设项目，也是后面在艾莉冒险中使用的基本参照对象，两者比较大的不同在于控制模式，将采用playerController的方式来制作，参考其他的项目，手游方面则是由于艾莉冒险在设计之初动作和操作并不复杂，无缝衔接过去。  
骑士的梦演示链接：https://www.bilibili.com/video/BV1oU4y1g7aA/  
有什么问题可以发邮件136191898@qq.com，目前游戏客户端求职中！！！

![7}E_2GFANV}PABH1UXO`Q%5](https://github.com/ArashiHF/myKnight/assets/56665189/41833ef6-0d63-4e48-87f4-27abe62cbadb)








