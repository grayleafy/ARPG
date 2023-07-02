# ARPG
Unity3D ARPG小游戏 实现了对话、任务、背包、武器系统，可以存档读档，有简短的剧情，行为树控制角色AI，IK控制脚贴地，AnimationRigging控制持枪瞄准和武器切换。

学习Unity后的第一个探索性质（探索游戏开发之道）的ARPG游戏，大部分系统都简单的实现了，很多东西在实现的过程中有了新的理解，所以后期觉得写得不够好然后摆烂准备下一个项目
## 一些不完全的简单介绍
对话系统：每一条对话后有不同的回复选项，每一个选项包含多个跳转，每一个跳转绑定一些前置条件，从而实现分支对话

任务系统：一个任务基于一系列子任务节点，任务节点之间是拓扑关系，每一个任务节点可以设置触发条件和结束条件，触发事件和结束事件，从而实现比较高的任务监听的自由度

武器：使用AnimationRigging控制武器的切换和瞄准，可以绑定不同的动画和约束，不同武器可以绑定各自的攻击、瞄准、射击三种事件

AI：基于行为树控制，自己实现了一些行为树的常用节点

## 注意事项
字体文件太大了上传不了，需要自行替换

需要安装InputSystem，AnimationRigging，以及Unity多态序列化扩展https://github.com/mackysoft/Unity-SerializeReferenceExtensions

## 游戏截图
![屏幕截图 2023-07-02 111902](https://github.com/grayleafy/ARPG/assets/86156654/bc54363b-0958-41c8-aee2-fd3bb89769c0)
![屏幕截图 2023-07-02 111937](https://github.com/grayleafy/ARPG/assets/86156654/02d94dd1-95a8-4c65-a641-7f7a996d8880)
![屏幕截图 2023-07-02 111957](https://github.com/grayleafy/ARPG/assets/86156654/52ba46dd-a562-41ee-b83f-f523463c48ef)
![屏幕截图 2023-07-02 112010](https://github.com/grayleafy/ARPG/assets/86156654/5c235c48-3b29-4670-b21f-22aa22249f87)
![屏幕截图 2023-07-02 112037](https://github.com/grayleafy/ARPG/assets/86156654/76a3d39c-5b3f-474a-a181-dbfb824f3439)
![屏幕截图 2023-07-02 112138](https://github.com/grayleafy/ARPG/assets/86156654/cb473b23-ddcf-42e6-92ef-d857e1a39b87)
![屏幕截图 2023-07-02 112243](https://github.com/grayleafy/ARPG/assets/86156654/39107043-79c0-47e3-8b93-3b8ac6b756f7)
![屏幕截图 2023-07-02 112209](https://github.com/grayleafy/ARPG/assets/86156654/c68a1a7a-83f2-4a4d-b9c0-e52544175be3)
![屏幕截图 2023-07-02 112403](https://github.com/grayleafy/ARPG/assets/86156654/177de95f-b6cd-401c-aeb4-852879ab7f37)
![屏幕截图 2023-07-02 112434](https://github.com/grayleafy/ARPG/assets/86156654/c8d41b04-e480-4625-a85a-e924e5ec5494)
![屏幕截图 2023-07-02 112500](https://github.com/grayleafy/ARPG/assets/86156654/0c8e4e26-423c-411a-95d1-2112163879fd)
![屏幕截图 2023-07-02 112523](https://github.com/grayleafy/ARPG/assets/86156654/09879107-66b3-4d00-b805-562a8e9adcf4)
![屏幕截图 2023-07-02 112543](https://github.com/grayleafy/ARPG/assets/86156654/e94f70d7-bd9a-4f2f-b686-0be8101d880a)
![屏幕截图 2023-07-02 112645](https://github.com/grayleafy/ARPG/assets/86156654/e002e651-799d-45b8-826c-03b5cc46c772)

