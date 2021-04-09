<?xml version="1.0" encoding="UTF-8"?>
<tileset name="Collisions" tilewidth="8" tileheight="8" tilecount="25" columns="5">
 <image source="../Tiles/Collisions.png" width="40" height="40"/>
 <tile id="0">
  <properties>
   <property name="Pit" type="bool" value="false"/>
   <property name="Solid" type="bool" value="false"/>
  </properties>
  <objectgroup draworder="index"/>
 </tile>
 <tile id="1">
  <properties>
   <property name="Pit" type="bool" value="false"/>
   <property name="Solid" type="bool" value="true"/>
  </properties>
  <objectgroup draworder="index"/>
 </tile>
 <tile id="2">
  <properties>
   <property name="Pit" type="bool" value="true"/>
   <property name="Solid" type="bool" value="false"/>
  </properties>
  <objectgroup draworder="index"/>
 </tile>
 <tile id="3">
  <properties>
   <property name="Pit" type="bool" value="false"/>
   <property name="Solid" type="bool" value="true"/>
  </properties>
  <objectgroup draworder="index"/>
 </tile>
</tileset>
