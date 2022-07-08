<!--
 * @Description: 分页组件
 * @Author: weig
 * @Date: 2021-07-02 08:31:31
 * @LastEditors: weig
 * @LastEditTime: 2021-10-11 15:12:43
-->
<template>
    <div class="pagination">
        <el-pagination
            @size-change="handleSizeChange"
            @current-change="handlePageChange"
            :current-page="state.pagination.currentpage"
            :page-sizes="pagesizes"
            :page-size="state.pagination.pagesize"
            :layout="state.currentLayout"
            :total="total"
            background>
        </el-pagination>
    </div>
</template>
<script>
import {reactive} from "vue"
export default{
    name: "EupPagination",
    props:{
        currentpage:{
            Type: Number,
            default: 1
        },
        pagesizes :{
            Type: Array,
            default: [10,20,50,100]
        },
        pagesize :{
            Type: Number,
            default: 10
        },
        total: {
            Type: Number,
            default: 0
        },
        layout: {
            Type: String,
            default: "total, sizes, prev, pager, next, jumper"
        }
    },
    setup(props, context){
        const layouts = {
            full: 'total, slot, sizes, prev, jumper, next',
            fullPager: 'total, slot, sizes, prev, pager, next',
            simple: 'prev, next',
            simpleJumper: 'prev, jumper, next'
        };
        const state = reactive({
            pagination:{
                currentpage: 1,
                pagesize: props.pagesize
            },
            currentLayout: layouts[props.layout] || props.layout,
        });

        // 分页导航
        const handlePageChange=(val)=> {
            state.pagination.currentpage=val
            context.emit("resPageData", {currPage: state.pagination.currentpage,pageSize: state.pagination.pagesize});
            context.emit("getPageData");//在子组件中调用父组件的方法
        }
        //改变页大小
        const handleSizeChange = (val) =>{
            state.pagination.pagesize = val;
            context.emit("resPageData", {currPage: state.pagination.currentpage,pageSize: state.pagination.pagesize});
            context.emit("getPageData");
        }
        //上一页
        const handlePrevClick = ((currentPage)=>{
            state.pagination.currentpage = currentPage;
            context.emit("getPageData");
        });
        //下一页
        const handleNextClick = ((currentPage)=> {
            state.pagination.currentpage = currentPage;
            context.emit("getPageData");
        });

        return{
            state,
            handlePageChange,
            handleSizeChange,
            handlePrevClick,
            handleNextClick
        }
    }
}
</script>
