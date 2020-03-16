<template>
    <main class="app-main">
        <el-scrollbar ref="scrollbar" class="scroll-container">
            <div class="page-view">
                <transition :name="transitionName" mode="out-in">
                    <keep-alive :include="cachedViews">
                        <router-view/>
                    </keep-alive>
                </transition>
            </div>
        </el-scrollbar>
        <el-backtop v-if="showBackToTop" target=".app-main .el-scrollbar__wrap" :visibility-height="400"/>
    </main>
</template>

<script>
    import { mapState } from 'vuex'

    export default {
        name: 'AppMain',
        computed: {
            ...mapState('tagsView', {
                cachedViews: state => state.cachedViews,
                transitionName: state => state.transitionName
            }),
            ...mapState('app', {
                scrollTop: state => state.scrollTop
            }),
            ...mapState('setting', {
                showBackToTop: state => state.showBackToTop
            })
        },
        watch: {
            scrollTop(v) {
                if (v >= 0) this.$refs.scrollbar.$refs['wrap'].scrollTop = v
            }
        }
    }
</script>

<style lang="scss">
    .app-main {
        position: relative;
        overflow: hidden;
        background-color: #f5f7f9;
        flex: 1;

        .scroll-container {
            position: relative;
            height: 100%;

            .page-view {
                margin: $pageViewMargin;
            }

            > .el-scrollbar__bar.is-horizontal {
                display: none !important;
            }

            > .el-scrollbar__wrap {
                display: flex;
                flex-direction: column;
                overflow-x: hidden;

                .el-scrollbar__view {
                    flex: 1;
                }
            }
        }
    }
</style>
