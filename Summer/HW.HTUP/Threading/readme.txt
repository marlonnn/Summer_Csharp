﻿多线程处理，负责发送、接受数据，并对接受到的数据进行处理，处理完成后放到队列中。

后续处理由各观察者完成。

对外的接口有两个：
（1）发送：调用txQueue，直接将数据push进去即可；
（2）接收：注册一个观察者，有新数据到达时会自动调用；