<!--
 * @Description: 
 * @Author: weig
 * @Date: 2021-08-25 13:41:58
 * @LastEditors: weig
 * @LastEditTime: 2021-10-11 14:59:17
-->
<template>
  <transition
    name="dialog-fade"
    @after-enter="afterEnter"
    @after-leave="afterLeave"
  >
    <div
      v-show="visible"
      class="el-dialog__wrapper"
      @click="handleWrapperClick"
    >
      <div
        :key="key"
        ref="dialog"
        role="dialog"
        aria-modal="true"
        :aria-label="title || 'dialog'"
        :class="['el-dialog', { 'is-fullscreen': fullscreen, 'el-dialog--center': center }, customClass]"
        :style="style"
      >
        <eup-container header-style="padding:0px;" footer-style="padding:0px;">
          <template #header>
            <div class="el-dialog__header">
              <slot name="title">
                <span class="el-dialog__title">{{ title }}</span>
              </slot>
              <div class="el-dialog__headertool">
                <i
                  v-if="showMaximize"
                  type="button"
                  aria-label="Close"
                  @click="handleMaximize"
                  :class="['el-dialog__close', 'el-icon', fullscreen ? 'el-icon-copy-document' : 'el-icon-full-screen']" 
                />
                <i
                  v-if="showClose"
                  type="button"
                  aria-label="Close"
                  @click="handleClose"
                  class="el-dialog__close el-icon el-icon-close" 
                />
              </div>
            </div>
          </template>
          <div v-if="rendered" class="el-dialog__body"><slot /></div>
          <template #footer>
            <div v-if="$slots.footer" class="el-dialog__footer">
              <slot name="footer" />
            </div>
          </template>
        </eup-container >
      </div>
    </div>
  </transition>
</template>

<script>
import { reactive, toRefs, onBeforeMount, onMounted,computed, watch,ref,onBeforeUnmount } from 'vue'
// import Popup from 'element-plus/lib/utils/popup/usePopup'
// // import Migrating from 'element-plus/lib/mixins/migrating'
// import Migrating from 'element-plus/packages/utils/mig'
// // import emitter from 'element-plus/src/mixins/emitter'
import EupContainer from '@/components/eup-container'
export default {
  name: 'EupElDialog',
  components:{
      EupContainer
  },
// mixins:[Popup, Migrating, emitter],
// mixins:[Popup],
  props:{
    title: {
      type: String,
      default: ''
    },

    modal: {
      type: Boolean,
      default: true
    },

    modalAppendToBody: {
      type: Boolean,
      default: true
    },

    appendToBody: {
      type: Boolean,
      default: false
    },

    lockScroll: {
      type: Boolean,
      default: true
    },

    closeOnClickModal: {
      type: Boolean,
      default: true
    },

    closeOnPressEscape: {
      type: Boolean,
      default: true
    },

    showMaximize: {
      type: Boolean,
      default: true
    },

    showClose: {
      type: Boolean,
      default: true
    },

    width: {
      type: String,
      default: null
    },

    fullscreen: Boolean,

    customClass: {
      type: String,
      default: ''
    },

    top: {
      type: String,
      default: '15vh'
    },
    beforeClose: {
      type: Function,
      default: null
    },
    center: {
      type: Boolean,
      default: false
    },

    destroyOnClose: Boolean,
    rendered:{
        type: Boolean,
        default:false
    }
  },
  emits: ["open","update","close","closed","opened"],
  setup(props, context) {
    const dialog = ref(null);
    const data = reactive({
        closed: false,
        key: 0,
        cacheStyle: {
            left: null,
            top: null,
            marginLeft: null,
            marginTop: this.top,
            width: this.width,
            height: null
        }
    });
    onBeforeMount(() => {
    });
    onMounted(() => {
        if (props.visible){
            props.rendered = true;
            dialog.value.open();
            if (props.appendToBody){
                document.body.appendChild(dialog.value.$el);
            }
        }
    });
    const style = computed(()=>{
        return style_func();
    });

    const visible = watch(()=>{
        return visible_func(val);
    });

    onBeforeUnmount(()=>{
        if (props.appendToBody && dialog.value.$el && dialog.value.$el.parentNode){
            dialog.value.$el.parentNode.removeChild(dialog.value.$el);
        }
    });

    /**
     * @description
     * @author weig
     */
    const style_func = ()=> {
        let style = {};
        if (!props.fullscreen) {
            style = { ...props.cacheStyle }
        } else {
            const dialogStyle = dialog.value.$refs.dialog.style
            data.cacheStyle.left = dialogStyle.left
            data.cacheStyle.top = dialogStyle.top
            data.cacheStyle.marginLeft = dialogStyle.marginLeft
            data.cacheStyle.marginTop = dialogStyle.marginTop
            data.cacheStyle.width = dialogStyle.width
            data.cacheStyle.height = dialogStyle.height
        }
        return style;
    }

    /**
     * @description
     * @author weig
     * @param {*} val
     */
    const visible_func =(val)=>{
        if (val){
            data.closed = false;
            context.emit('open');
            dialog.value.$el.addEventListener('scroll', updatePopper);
            dialog.value.$nextTick(()=>{
                dialog.value.$refs.dialog.scrollTop = 0;
            });
            if (props.appendToBody){
                document.body.appendChild(dialog.value.$el);
            }
        } else {
            dialog.value.$el.removeEventListener('scroll', updatePopper);
            if (!data.closed) context.emit('close');
            if (props.destroyOnClose){
                dialog.value.$nextTick(()=>{
                    data.key++;
                });
            }
        }
    }

    /**
     * @description
     * @author weig
     */
    const updatePopper = ()=>{
        dialog.value.broadcast('ElSelectDropdown', 'updatePopper');
        dialog.value.broadcast('ElDropdownMenu', 'updatePopper')
    }

    /**
     * @description
     * @author weig
     */
    const getMigratingConfig = ()=>{
        return {
            props:{
                'size': "size is remived."
            }
        }
    }

    /**
     * @description
     * @author weig
     */
    const handleWrapperClick =()=>{
        if (!props.closeOnClickModal) return;
        handleClose();
    }

    /**
     * @description
     * @author weig
     */
    const handleClose =()=>{
        if (typeof props.beforeClose === 'function'){
            beforeClose(hide)
        } else {
            hide();
        }
    }

    /**
     * @description
     * @author weig
     * @param {Boolean} cancel
     */
    const hide =(cancel)=>{
        if (cancel !== false){
            context.emit('update', false);
            context.emit('close');
            data.closed = true;
        }
    }

    /**
     * @description
     * @author weig
     */
    const handleMaximize =()=>{
        context.emit("update", !props.fullscreen);
    }

    /**
     * @description
     * @author weig
     */
    const afterEnter= ()=>{
        context.emit('opened');
    }

    /**
     * @description
     * @author weig
     */
    const afterLeave= ()=>{
        context.emit('closed');
    }
    return {
      ...toRefs(data),
      style,
      dialog,
      getMigratingConfig,
      visible,
      handleWrapperClick,
      handleMaximize,
      afterEnter,
      afterLeave
    }
  },
}

</script>
<style scoped lang='less'>
</style>