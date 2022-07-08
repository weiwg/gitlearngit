<!--
 * @Description: 
 * @Author: 优
 * @Date: 2021-9-24 15:47:11
 * @LastEditors: weig
 * @LastEditTime: 2021-12-09 13:45:47
-->
<template>
  <div>
       <div id="containerrete" ></div>
    <div id="panel"></div>
  </div>
</template>

<script>
import { reactive, onBeforeMount, onMounted } from 'vue'
import AMap from 'AMap' 
export default {
  name: 'EupAmap',
   props: [
    'amap'
  ],
  setup(props, context) {
    const state = reactive({})
    onBeforeMount(() => {
    })
    onMounted(() => {
      var sted=props.amap.startCoordinate.split(',')
      var endleng=props.amap.wayCoordinates.length-1
      var ened=props.amap.wayCoordinates[endleng]
      if(!ened.endCoordinate){
        return
      }
      var endCoordinate=ened.endCoordinate.split(',')
      let nuber=[]
      for (let index = 0; index < props.amap.wayCoordinates.length-1; index++) {
         let user=props.amap.wayCoordinates[index].endCoordinate
             let ender=user.split(',')
             nuber.push( new AMap.LngLat(ender[0],ender[1]))       
      }
      var map = new AMap.Map("containerrete", {
        resizeEnable: true,
        zoom: 13
    });
      AMap.service('AMap.Driving',function(){//回调函数
        var driving= new AMap.Driving({
            map: map,
        });   
        driving.search(new AMap.LngLat(sted[0], sted[1]), new AMap.LngLat(endCoordinate[0], endCoordinate[1]),{
          waypoints:nuber
        });
    })
    })
    return {
      state
    }
  },
}

</script>
<style>
</style>