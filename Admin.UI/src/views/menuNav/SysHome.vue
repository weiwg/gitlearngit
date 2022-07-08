<template>
    <div>
        <h1>{{title}}</h1>
    </div>
</template>

<script>
import {useStore} from 'vuex'
export default {
    name: "SysHome",
    data() {
        return{
            title: "后台管理系统"
        } 
    },
    components: {
    },
    computed: {
    },

    methods: {
    },
    mounted(){
    }
};
</script>

<style scoped>
</style>
