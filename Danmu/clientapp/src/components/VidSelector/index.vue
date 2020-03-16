<template>
    <el-select :value="value" filterable clearable allow-create @change="$emit('input',$event)">
        <el-option v-for="vid in vids" :key="vid" :label="vid" :value="vid"/>
    </el-select>
</template>

<script>
    import { mapState } from 'vuex'
    import { getAllVids } from '@/api/admin/danmu'

    export default {
        name: 'index',
        props: {
            value: String
        },
        computed: {
            ...mapState('dataCache', {
                vids: state => state.searchedVids
            })
        },
        mounted() {
            if (this.vids.length > 0) return
            getAllVids().then(data => this.$store.commit('dataCache/setSearchedVids', data))
        }
    }
</script>
