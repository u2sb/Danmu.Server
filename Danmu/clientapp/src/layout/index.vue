<template>
    <section class="el-container app-wrapper">
        <v-sidebar/>
        <section class="el-container main-container">
            <header class="header-container">
                <v-navbar/>
                <tags-view/>
            </header>
            <v-main/>
        </section>
        <!--移动端侧边栏展开时的遮罩-->
        <div v-show="showSidebarMask" class="drawer-bg" @click="$store.commit('app/setCollapseSidebar',true)"/>
    </section>
</template>

<script>
    import VMain from './components/Main'
    import VNavbar from './components/Navbar'
    import TagsView from './components/TagsView'
    import VSidebar from './components/Sidebar'
    import { mapState } from 'vuex'
    import ResizeHandler from './mixin/ResizeHandler'

    export default {
        name: 'Layout',
        mixins: [ResizeHandler],
        components: { VMain, VSidebar, VNavbar, TagsView },
        computed: {
            ...mapState('app', {
                device: state => state.device,
                collapseSidebar: state => state.collapseSidebar
            }),
            showSidebarMask() {
                return !this.collapseSidebar && this.device === 'mobile'
            }
        }
    }
</script>

<style lang="scss">
    .app-wrapper {
        height: 100%;
        width: 100%;
        flex-direction: row;

        .main-container {
            overflow: hidden;
            position: relative;
            flex-direction: column;

            .header-container {
                height: $headerHeight;
                transition: height .3s ease-in-out;
                flex-shrink: 0;
            }
        }

        .drawer-bg {
            background: #000;
            opacity: 0.3;
            width: 100%;
            top: 0;
            height: 100%;
            position: absolute;
            z-index: 9;
        }
    }
</style>
