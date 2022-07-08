<template>
<div>
    <div ref='editor' style="text-align:left">
    </div>
</div>
</template>
<script>
import {onMounted,ref,watch,reactive } from 'vue'
import WangEditor from 'wangeditor'
import { ElMessage } from 'element-plus'
import {postimg} from "@/serviceApi/Image/Image"
export default {
 props:{
   contant:{required:true},
 },
  name:'Editor',
   setup(props, context) {
      
    /*   watch(()=>props.contant,(newProps,oldProps)=> {
    if(!state.isChange){
       console.log(newProps);
       instance.txt.html(newProps);
       }
       else{
        console.log(newProps);
        state.isChange=false
       } 
        /* instance.txt.html(newProps)  */
    /*    instance.selection.moveCursor(instance.$textElem.elems[0],true);  
    })  */
   watch(()=>props.contant, (newProps,oldProps) =>{
                if (props.contant !== instance.txt.html()) {
                     instance.txt.html(newProps);    //根据父组件传来的值设置html值
                }
               /*  instance.selection.moveCursor(instance.$textElem.elems[0],false); */
                /* console.log(instance.selection.getSelectionContainerElem().elems[0]); */
            }
            //value为编辑框输入的内容，这里我监听了一下值，当父组件调用得时候，如果给value赋值了，子组件将会显示父组件赋给的值
        )
    const editor = ref(null)
       const state = reactive({
       isChange: false  
       })
    let instance
    onMounted(() => {
      instance = new WangEditor(editor.value)
      instance.config.showLinkImg = false
      instance.config.showLinkImgAlt = false
      instance.config.showLinkImgHref = false
      instance.config.uploadImgMaxLength = 1;
      instance.config.height = 500
      instance.config.showLinkVideo =true
      instance.config.lineHeights = ['1', '1.15', '1.6', '2', '2.5', '3']
      instance.config.uploadImgMaxSize = 2 * 1024 * 1024 // 2M 
      instance.config.excludeMenus = [
        'code',
        'undo',
        'redo',
        'todo',
    ];

      // 图片返回格式不同，这里需要根据后端提供的接口进行调整   customUpload
      instance.config.customUploadImg = function(resultFiles, insertImgFn) {
            var formData = new FormData();
            formData.append('File',resultFiles[0]);
            formData.append('FileCategory',200);
           postimg(formData).then(function(res){
          if (res.code==1) {
             insertImgFn(res.data.url) 
          }
          else{
            ElMessage.error(res.msg);
          }
           }
           )}
           instance.config.onchange = html => {
           state.isChange=true; 
            context.emit('updateContent',html); 
    };
     instance.create()
    })
        return {
            editor,
        } 
   }
}
</script>
