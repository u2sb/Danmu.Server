<template>
    <nav class="navbar">
        <hamburger :is-active="!collapseSidebar" @click="clickHamburger"/>
        <div class="right-menu">
            <el-dropdown
                    class="avatar-container right-menu-item"
                    trigger="click"
                    @visible-change="$emit('menu-show',$event)"
            >
                <div class="avatar-wrapper">
                    <span>{{name}}</span>
                    <i class="el-icon-caret-bottom"/>
                </div>
                <el-dropdown-menu class="user-dropdown" slot="dropdown">
                    <router-link to="/user">
                        <el-dropdown-item icon="el-icon-user">个人中心</el-dropdown-item>
                    </router-link>
                    <el-dropdown-item
                            divided
                            icon="el-icon-switch-button"
                            @click.native="logout"
                    >
                        退出登录
                    </el-dropdown-item>
                </el-dropdown-menu>
            </el-dropdown>
        </div>
    </nav>
</template>

<script>
    import Hamburger from './components/Hamburger'
    import { mapState } from 'vuex'
    import { elConfirm } from '@/utils/message'

    export default {
        name: 'navbar',
        components: { Hamburger },
        computed: {
            ...mapState('app', {
                collapseSidebar: state => state.collapseSidebar
            }),
            ...mapState('user', {
                name: state => state.name
            })
        },
        methods: {
            clickHamburger() {
                this.$store.commit('app/setCollapseSidebar', !this.collapseSidebar)
            },
            logout() {
                elConfirm('确认退出?').then(() => this.$store.dispatch('user/logout'))
            }
        }
    }
</script>

<style lang="scss">
    @import "~@/assets/styles/navbar.scss";
</style>
