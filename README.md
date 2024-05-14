# MyOrange_MySql
## 简介 
   ### 一个简单的在.Net平台下使用C#写的ORM和工具类的集合项目。主要采用反射来完成DataTable和属性的转换，可以使用于Web和Form。
   ### BaseModel 类是一个基类所有的Model可以继承此类，自动拥有增、删、查、改的功能；表名和类名对应，总是应当使用Id做主键，预先生成属性，如果不想对应到数据表的字段，可以使用public 定义。  
   ### Common 定义一些共有的方法。
   ### Config 使用dataset读取配置文件(xml格式) 。 
   ### DB 通用数据库类，通过反射得到集合然后拼装为SQL语句再操作数据库。  
   ### DBAttribute 属性定义类，可以使用在model上定义一个属性是否为主键或者拼装的时候是否忽略。
   ### ImageClass 图片通用处理类，包括保存，缩略图，水印 。
   ### ImageModification 水印类，不是我写的 。
   ### Web 封装了一些Web环境常用的方法，类似替换html代码，左右截断。
   ### Xls 使用ODBC读取Xls文件和把DataTable转为csv下载的工具类，ODBC的方式不适用于服务器端，因为可能没有这个驱动。


***
## 安装和设置 
+ ### 需要MySql的dll
+ ### 直接下载源码编译即可，我是用visual studio 2022 社区版编译的
+ ### 使用的时候直接dll

***


## 使用说明 
直接使用 
***
## 其他说明
+ ### 简单的ORM使用DateTable做数据传递，Model不支持类与类的引用映射，比如一对一等。
+ ### 现在使用ef估计更简单
