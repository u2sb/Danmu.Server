<template>
    <el-dialog
            v-drag-dialog
            class="dialog-form"
            :top="top"
            :visible="value"
            :width="width"
            @close="$emit('close')"
            @open="$emit('open')"
    >
        <slot slot="title" name="title">{{title}}</slot>
        <div class="dialog-form-container">
            <loading-mask :show="loading"/>
            <div class="dialog-form-main">
                <slot/>
            </div>
        </div>
        <slot slot="footer" name="footer"/>
    </el-dialog>
</template>

<script>
    import LoadingMask from '@/components/LoadingMask'

    export default {
        name: 'DialogForm',
        components: { LoadingMask },
        props: {
            value: { type: Boolean, default: false },
            top: { type: String, default: '50px' },
            width: { type: String, default: '70%' },
            loading: { type: Boolean, default: true },
            title: { type: String, default: '' }
        }
    }
</script>

<style lang="scss">
    .dialog-form {
        .el-dialog__body {
            padding: 0;
        }

        .dialog-form-container {
            position: relative;
            width: 100%;

            .dialog-form-main {
                padding: 20px 20px 0;
                max-height: calc(100vh - 53px - 53px - 100px);
                overflow: auto;
            }

            .el-form {
                padding: 0;
            }
        }
    }
</style>
