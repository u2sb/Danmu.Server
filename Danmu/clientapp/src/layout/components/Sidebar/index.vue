<template>
    <aside :class="asideClass">
        <logo v-if="showLogo" :collapse="collapse"/>
        <el-scrollbar>
            <el-menu
                    ref="menu"
                    :active-text-color="variables.primary"
                    :background-color="variables.menuBg"
                    :collapse="collapse"
                    :collapse-transition="false"
                    :default-active="$route.path"
                    :text-color="variables.menuText"
                    :unique-opened="sidebarUniqueOpen"
                    mode="vertical"
                    @select="select"
            >
                <sidebar-item
                        v-for="route in routes"
                        :key="route.path"
                        :show-parent="sidebarShowParent"
                        :collapse="collapse"
                        :item="route"
                />
            </el-menu>
        </el-scrollbar>
    </aside>
</template>

<script>
    import { mapState } from 'vuex'
    import Logo from './components/Logo'
    import SidebarItem from './components/SidebarItem'
    import variables from '@/assets/styles/variables.scss'

    export default {
        name: 'sidebar',
        components: { SidebarItem, Logo },
        data: () => ({ variables }),
        computed: {
            ...mapState('resource', {
                routes: state => state.sidebarMenus
            }),
            ...mapState('setting', {
                showLogo: state => state.showLogo,
                sidebarUniqueOpen: state => state.sidebarUniqueOpen,
                sidebarShowParent: state => state.sidebarShowParent
            }),
            ...mapState('app', {
                device: state => state.device,
                collapseSidebar: state => state.collapseSidebar
            }),
            //仅在pc端可折叠
            collapse() {
                return this.collapseSidebar && this.device === 'pc'
            },
            //仅在移动端可隐藏
            hide() {
                return this.collapseSidebar && this.device === 'mobile'
            },
            asideClass() {
                return {
                    'sidebar-container': true,
                    'mobile': this.device === 'mobile',
                    'collapse-sidebar': this.collapse,
                    'hide-sidebar': this.hide
                }
            }
        },
        methods: {
            select(index) {
                if (this.$route.path === index) {
                    this.$store.commit('tagsView/DEL_CACHED_VIEW', this.$route)
                    this.$nextTick(() => this.$router.replace({ path: '/redirect' + this.$route.fullPath }))
                }
                else this.$router.push(index)
            }
        }
    }
</script>
