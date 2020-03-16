<template>
    <el-card class="user-card">
        <el-tabs v-model="activeTab" :tab-position="tabPosition" stretch>
            <el-tab-pane v-for="{label,name} in tabs" :key="name" :name="name" :label="label"/>
            <div :class="tabContentClass">
                <component :is="activeTab"/>
            </div>
        </el-tabs>
    </el-card>
</template>

<script>
    import UserInfo from './components/UserInfo'
    import ChangePwd from './components/ChangePwd'
    import { mapState } from 'vuex'

    const tabs = [
        { label: '个人信息', name: 'user-info' },
        { label: '修改密码', name: 'change-pwd' }
    ]

    export default {
        name: 'user',
        components: { UserInfo, ChangePwd },
        computed: {
            ...mapState('app', {
                tabPosition: state => state.device === 'pc' ? 'left' : 'top'
            }),
            tabContentClass() {
                return this.tabPosition === 'left' ? 'tab-main-right' : 'tab-main-top'
            }
        },
        data() {
            return {
                activeTab: tabs[0].name,
                tabs
            }
        }
    }
</script>

<style lang="scss">
    .user-card {
        .el-tabs--left {
            .el-tabs__item.is-left {
                text-align: left;

                &.is-active {
                    background: #f0faff;
                }
            }

            .el-tabs__header.is-left {
                min-width: 200px;
            }
        }

        .tab-main {
            &-right {
                padding: 8px 40px;
            }

            &-bottom {
                padding: 20px 40px;
            }
        }
    }
</style>
