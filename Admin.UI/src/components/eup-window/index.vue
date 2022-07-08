<!--
 * @Description: 主体内容区域组件
 * @Author: weig
 * @Date: 2021-08-25 10:00:54
 * @LastEditors: weig
 * @LastEditTime: 2021-10-11 15:17:14
-->
<template>
    <el-drawer
        v-if="drawer"
        ref="mydrawer"
        v-resizable="currentDrawerResizeOptions"
        :modal="currentModal"
        :close-on-press-escape="closeOnPressEscape"
        :append-to-body="currentAppendToBody"
        :model-value="visible"
        destroy-on-close
        :direction="direction"
        :size="currentSize"
        :custom-class="customClass"
        :before-close="beforeClose?beforeClose:onCancel"
        @open="onOpen"
        @close="onClose"
        @opened="onOpened"
        @closed="onClosed"
    >
    <template #title>
    <slot name="title">
        <span role="heading" :title="title">{{ title }}</span>
    </slot>
    </template>
    <section :style="drawerBodyStyle">
    <slot />
    </section>
    <div class="drawer-footer">
    <slot name="footer">
        <el-button @click="onCancel">取消</el-button>
        <el-button type="primary" @click="onSure">确定</el-button>
    </slot>
    </div>
    </el-drawer>

    <eup-el-dialog
        v-else
        v-draggable="currentDragOptions"
        v-resizable="cuurentResizeOptions"
        :v-model="visible"
        :modal="currentModal"
        :modal-append-to-body="currentModalAppendToBody"
        :append-to-body="currentAppendToBody"
        :top="top"
        :custom-class="customClass"
        :close-on-click-modal="closeOnClickModal"
        :close-on-press-escape="closeOnPressEscape"
        :before-close="beforeClose?beforeClose:onCancel"
        :style="dialogStyle"
        :fullscreen="currentFullscreen"
        @open="onOpen"
        @close="onClose"
        @mousedown="onMousedown"
        ref="mydialog"
    >
        <template #title>
        <slot name="title">
            <span class="el-dialog__title">{{ title }}</span>
        </slot>
        </template>
        <slot />
        <template #footer>
        <slot name="footer">
            <el-button @click="onCancel">取消</el-button>
            <el-button type="primary" @click="onSure">确定</el-button>
        </slot>
        </template>
    </eup-el-dialog>
</template>

<script>
import { reactive, toRefs, onBeforeMount, onMounted,onBeforeUnmount,computed,ref } from 'vue'
import { addResizeListener, removeResizeListener } from 'element-plus/lib/utils/resize-event'
import { setStyle } from 'element-plus/lib/utils/dom'
import draggable from '@/directive/draggable'
import resizable from '@/directive/resizable'
import EupElDialog from '@/components/eup-dialog'
export default {
  name: 'EupWindow',
  components:{
      EupElDialog
  },
  directives:{
      draggable,
      resizable
  },
  props:{
        title: {
            type: String,
            default: ''
        },
        // 抽屉
        drawer: {
            type: Boolean,
            default: false
        },
        // 可拖拽
        draggable: {
            type: Boolean,
            default: true
        },
        // 可更改尺寸
        resizable: {
            type: Boolean,
            default: true
        },
        // 更改尺寸方向
        resizeHandles: {
            type: [String, Array],
            default: 'all'
        },
        // resize选项
        resizeOptions: {
            type: Object,
            default: null
        },
        // drag选项
        dragOptions: {
            type: Object,
            default: null
        },
        drawerResizeOptions: {
            type: Object,
            default: null
        },
        // 页脚可拖拽
        footerDraggable: {
            type: Boolean,
            default: true
        },
        visible: {
            type: Boolean,
            default: false
        },
        // 遮罩
        modal: {
            type: Boolean,
            default: true
        },
        // 窗口内嵌
        embed: {
            type: Boolean,
            default: false
        },
        // 窗口切换
        switch: {
            type: Boolean,
            default: false
        },
        // 全屏
        fullscreen: {
            type: Boolean,
            default: false
        },
        appendToBody: {
            type: Boolean,
            default: true
        },
        modalAppendToBody: {
            type: Boolean,
            default: true
        },
        closeOnClickModal: {
            type: Boolean,
            default: false
        },
        wrapperClosable: {
            type: Boolean,
            default: false
        },
        closeOnPressEscape: {
            type: Boolean,
            default: false
        },
        top: {
            type: String,
            default: '8vh'
        },
        customClass: {
            type: String,
            default: 'my-window'
        },
        direction: {
            type: String,
            default: 'btt',
            validator(val) {
                return ['ltr', 'rtl', 'ttb', 'btt'].indexOf(val) !== -1
            }
        },
        size: {
            type: [Number, String],
            default: 'auto'
        },
        drawerBodyStyle: {
            type: String,
            default: 'padding:24px 48px 74px 24px;'
        },
        beforeClose: {
            type: Function,
            default: null
        }
  },
  emits:["open","opened","close","closed","update","cancel","sure"],
  setup(props, context) {
    let drawerResizeHandles = props.currentResizeHandles;
    if (props.drawer){
        switch(props.direction){
            case 'btt':
                drawerResizeHandles = 'n';
                break;
            case 'ttb':
                drawerResizeHandles = 's';
                break;
            case 'ltr':
                drawerResizeHandles = 'e';
                break;
            case 'rtl':
                drawerResizeHandles = 'w';
                break;
            default:
                drawerResizeHandles = props.currentResizeHandles;
                break;
        }
    }
    const mydrawer = ref(null);
    const mydialog = ref(null);
    const data = reactive({
        currentFullscreen: props.fullscreen,
        currentModal: props.embed ? false : (props.modal && !props.switch),
        currentSize: props.fullscreen ? '100%' : props.size,
        drawerResizeHandles: drawerResizeHandles,
        currentAppendToBody: props.embed ? false : props.appendToBody,
        currentModalAppendToBody: props.embed ? false : props.modalAppendToBody
    });
    onBeforeMount(() => {});
    onMounted(() => {
      if (props.drawer){
          mydrawer.value.proxyChangeLayout = changeLayout.bind(mydrawer.value);
          addResizeListener();
      }
    });
    onBeforeUnmount(()=>{
        mydrawer.value.proxyChangeLayout && removeResizeListener(mydrawer.value.$el, mydrawer.value.proxyChangeLayout);
    });
    const currentDrawerResizeOptions = computed (() =>{
        return currentDrawerResizeOptions_func();
    });
    const drawerStyle = computed (() =>{
        return drawerStyle_func();
    });
    const currentDragOptions = computed(()=>{
        const handles = [];
        if (props.draggable) {
            handles.push('.el-dialog__header');
        }
        if (props.footerDraggable) {
            handles.push('.el-dialog__footer');
        }
        return {
            host: '.el-dialog',
            handle: handles,
            disabled: data.currentFullscreen || (!props.draggable && !props.footerDraggable),
            autoCalcRange: true,
            offset: {
            left: 'marginLeft',
            top: 'marginTop'
            },
            ...props.dragOptions
        }
    });
    const dialogStyle = computed(()=>{
        const style = {
            pointerEvents: props.switch ? 'none' : '',
            overflow: props.switch ? 'hidden' : 'auto'
        }

        if (props.embed) {
            style.position = 'absolute'
        }

        return style
    });
    const cuurentResizeOptions = computed(()=>{
        return {
            host: '.el-dialog',
            handles: props.resizeHandles,
            disabled: data.currentFullscreen || !props.resizable,
            offset: {
            left: 'marginLeft',
            top: 'marginTop'
            },
            minWidth: 200,
            minHeight: 190,
            autoCalcRange: true,
            ...props.resizeOptions
        }
    });

    /**
     * @description 当前抽屉重置大小选项
     * @author weig
     */
    const currentDrawerResizeOptions_func = ()=>{
        return {
            host: '.el-drawer',
            handles: data.drawerResizeHandles,
            disabled: data.currentFullscreen || !props.resizable,
            offset:{
                left: 'marginLeft',
                top: 'marginTop'
            },
            minWidth: 200,
            minHeight: 190,
            autoCalcRange: true,
            ...props.resizeOptions
        }
    };

    /**
     * @description 抽屉样式
     * @author weig
     */
    const drawerStyle_func=()=> {
        const style = {
            pointerEvents: props.switch ? 'none' : '',
            overflow: 'hidden'
        }

        if (props.embed) {
            style.position = 'absolute'
        }

        return style
    }

    /**
     * @description 改变布局
     * @author weig
     */
    const changeLayout =()=>{
        props.currentSize = props.size;
        mydrawer.value.$nextTick(function() {
            const rect = props.embed ? document.querySelector('.el-main.main').getBoundingClientRect() : document.body.getBoundingClientRect()
            const drawerRect = mydrawer.value.$refs.mydrawer.$refs.drawer.getBoundingClientRect()
            if (drawerRect.height > rect.height || drawerRect.width > rect.width) {
                data.currentFullscreen = true;
                props.currentSize = '100%';
            } else {
                data.currentFullscreen = false;
                props.currentSize = props.size;
            }
        })
    }

    /**
     * @description 打开
     * @author weig
     */
    const onOpen =()=>{
        context.emit('open');
    }

    /**
     * @description 
     * @author weig
     */
    const onOpened =()=>{
        context.emit('opened');
    }

    /**
     * @description 关闭
     * @author weig
     */
    const onClose =()=>{
        context.emit('close');
    }

    /**
     * @description
     * @author weig
     */
    const onClosed = ()=> {
        context.emit('closed');
    }

     /**
     * @description 取消
     * @author weig
     */
    const onCancel =()=> {
      context.emit('update', false)
      context.emit('cancel');
    }
    
     /**
     * @description 确定
     * @author weig
     */
    const onSure =()=>{
      context.emit('sure');
    }

    /**
     * @description 点击窗口，实现切换窗口切换功能
     * @author weig
     */
    const onMousedown = ()=>{
        if (props.switch) {
            const wins = [];
            const wrappers = mydialog.value.$el.parentNode.querySelectorAll('.el-dialog__wrapper');
            if (wrappers.length === 1) {
                return;
            }
            wrappers.forEach(function(el) {
                if (el.style.zIndex > 0) {
                    wins.push(el);
                }
            });
            if (wins.length === 1) {
                return;
            }

            wins.sort(function(a, b) {
                if (a.style.zIndex > b.style.zIndex) {
                    return 1;
                } else if (a.style.zIndex < b.style.zIndex) {
                    return -1;
                } else {
                    return 0;
                }
            });

            const zIndexs = wins.map(w => {
                return w.style.zIndex;
            });

            const currentZIndex = mydialog.value.$el.style.zIndex;
            const currentIndex = wins.findIndex(w => w.style.zIndex === currentZIndex);
            const deleteWins = wins.splice(currentIndex, 1);
            wins.push(deleteWins[0]);
            wins.forEach(function(w, index) {
                setStyle(w, 'z-index', zIndexs[index]);
            });
        }
    }
    return {
      ...toRefs(data),
      currentDrawerResizeOptions,
      drawerStyle,
      currentDragOptions,
      dialogStyle,
      cuurentResizeOptions,
      mydrawer,
      onOpen,
      onOpened,
      onClose,
      onClosed,
      onCancel,
      onSure,
      onMousedown
    }
  },
}

</script>
<style scoped lang='less'>
</style>
