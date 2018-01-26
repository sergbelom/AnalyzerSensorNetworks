library(ggplot2)
library(dplyr)

dataFromMEMS <- data.frame(read.csv('DataFromMems.csv' , sep = ' '))
str(dataFromMEMS)

dataMEMS = data.frame( Num = dataFromMEMS$X0 , ID = dataFromMEMS$X31236 , X = dataFromMEMS$X28156 , Y = dataFromMEMS$X34305 , Z = dataFromMEMS$X12032 )
str(dataMEMS)
qplot(dataMEMS$X)

gr_dataMEMS <- group_by(dataMEMS, ID)
gr_dataMEMS$ID <- factor(gr_dataMEMS$ID)
str(gr_dataMEMS)

dataMEMS_Sensor1 <- data.frame(dataMEMS[dataMEMS$ID == 17160,])
dataMEMS_Sensor2 <- data.frame(dataMEMS[dataMEMS$ID == 31236,])

ggplot(dataMEMS_Sensor1) +
  geom_line( aes( x = dataMEMS_Sensor1$Num , y = dataMEMS_Sensor1$X ) , color = "green" ) +
  geom_line( aes( x = dataMEMS_Sensor1$Num , y = dataMEMS_Sensor1$Y ) , color = "red" ) +
  geom_line( aes( x = dataMEMS_Sensor1$Num , y = dataMEMS_Sensor1$Z ) , color = "blue" )


ggplot(dataMEMS_Sensor2) +
  geom_line( aes( x = dataMEMS_Sensor2$Num , y = dataMEMS_Sensor2$X ) , color = "green" ) +
  geom_line( aes( x = dataMEMS_Sensor2$Num , y = dataMEMS_Sensor2$Y ) , color = "red" ) +
  geom_line( aes( x = dataMEMS_Sensor2$Num , y = dataMEMS_Sensor2$Z ) , color = "blue" )






Yplot <- ggplot(dataMEMS , aes( x = dataMEMS$Num , y = dataMEMS$Y ))+
  geom_point()+
  geom_line()

Zplot <- ggplot(dataMEMS , aes( x = dataMEMS$Num , y = dataMEMS$Z ))+
  geom_point()+
  geom_line()


 fft(dataMEMS$X)
