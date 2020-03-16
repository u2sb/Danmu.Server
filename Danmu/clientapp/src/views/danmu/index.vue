<template>
    <el-card v-loading="config.operating">
        <search-form>
            <search-form-item label="vid：">
                <vid-selector v-model="searchForm.vid"/>
            </search-form-item>
            <search-form-item label="开始时间：">
                <el-date-picker
                        v-model="searchForm.startDate"
                        format="yyyy-MM-dd HH:mm:ss"
                        type="datetime"
                        value-format="yyyy-MM-dd HH:mm:ss"
                />
            </search-form-item>
            <search-form-item label="结束时间：">
                <el-date-picker
                        v-model="searchForm.endDate"
                        format="yyyy-MM-dd HH:mm:ss"
                        type="datetime"
                        value-format="yyyy-MM-dd HH:mm:ss"
                />
            </search-form-item>
            <search-form-item label="用户名：">
                <el-input v-model="searchForm.author" clearable maxlength="50"/>
            </search-form-item>
            <search-form-item label="用户ID：">
                <el-input v-model="searchForm.authorId" clearable maxlength="50"/>
            </search-form-item>
            <search-form-item label="弹幕类型：">
                <el-select v-model="searchForm.mode" clearable @clear="searchForm.mode=null">
                    <el-option v-for="{value,label} in danmuModes" :key="value" :value="value" :label="label"/>
                </el-select>
            </search-form-item>
            <search-form-item label="ip：">
                <el-input v-model="searchForm.ip" clearable maxlength="19"/>
            </search-form-item>
            <search-form-item label="key：">
                <el-input v-model="searchForm.key" clearable maxlength="50"/>
            </search-form-item>
            <search-form-item label="排序：">
                <el-radio-group v-model="searchForm.descending">
                    <el-radio :label="true">降序</el-radio>
                    <el-radio :label="false">升序</el-radio>
                </el-radio-group>
            </search-form-item>
        </search-form>
        <el-row>
            <el-button size="small" type="success" @click="search">查 询</el-button>
            <el-button size="small" type="primary" @click="edit">编 辑</el-button>
            <el-button size="small" type="danger" @click="del">删 除</el-button>
        </el-row>
        <el-row v-loading="config.loading" class="table-container">
            <el-table
                    ref="table"
                    :data="tableData"
                    current-row-key="id"
                    highlight-current-row
                    row-key="id"
                    @row-click="row=$event"
            >
                <el-table-column align="center" type="expand" width="40">
                    <el-form slot-scope="{row}" label-position="right" label-width="85px" size="small">
                        <el-row>
                            <el-col
                                    v-for="{label,value} in formatDanmuData(row.data)"
                                    :key="label"
                                    :sm="12"
                                    :lg="8"
                                    :xl="6"
                            >
                                <el-form-item :label="label">{{value}}</el-form-item>
                            </el-col>
                        </el-row>
                    </el-form>
                </el-table-column>
                <el-table-column align="center" label="#" type="index" width="80"/>
                <el-table-column align="center" label="vid" prop="vid" show-overflow-tooltip/>
                <el-table-column align="center" label="弹幕内容" prop="data.text" show-overflow-tooltip/>
                <el-table-column align="center" label="创建时间" width="180" show-overflow-tooltip>
                    <template v-slot="{row}">{{getLocalTime(row.createTime)}}</template>
                </el-table-column>
                <el-table-column align="center" label="修改时间" width="180" show-overflow-tooltip>
                    <template v-slot="{row}">{{getLocalTime(row.updateTime)}}</template>
                </el-table-column>
                <el-table-column align="center" label="ip" prop="ip" width="150" show-overflow-tooltip/>
                <el-table-column align="center" label="是否删除" width="100">
                    <template v-slot="{row}">{{row.isDelete?'是':'否'}}</template>
                </el-table-column>
            </el-table>
            <el-pagination
                    background
                    :pager-count="5"
                    :current-page="searchForm.page"
                    :page-size="searchForm.size"
                    :total="searchForm.total"
                    :layout="paginationLayout"
                    @current-change="pageChange"
            />
        </el-row>
        <edit-dialog v-model="editDialog" :id="row?row.id:null" :type="type" @success="success"/>
    </el-card>
</template>

<script>
    import SearchForm from '@/components/SearchForm'
    import SearchFormItem from '@/components/SearchForm/SearchFormItem'
    import VidSelector from '@/components/VidSelector'
    import EditDialog from './EditDialog'
    import { isEmpty } from '@/utils'
    import { getLocalTime } from '@/utils/date'
    import { elConfirm, elError, elSuccess } from '@/utils/message'
    import tableMixin from '@/mixins/tablePageMixin'
    import { danmuModes, danmuDataDefine } from './constant'
    import { search, del } from '@/api/admin/danmu'

    export default {
        name: 'danmuList',
        mixins: [tableMixin],
        components: { SearchForm, SearchFormItem, VidSelector, EditDialog },
        data() {
            return {
                searchForm: {
                    vid: '',
                    startDate: '',
                    endDate: '',
                    author: null,
                    authorId: null,
                    mode: null,
                    ip: null,
                    key: null,
                    descending: true
                },
                danmuModes,
                editDialog: false
            }
        },
        methods: {
            getLocalTime,
            formatDanmuData(data) {
                const res = []
                for (const key of Object.keys(data)) {
                    if (['text'].includes(key)) continue
                    res.push({ label: danmuDataDefine[key], value: data[key] })
                }
                return res
            },
            search() {
                if (this.config.loading) return
                this.config.loading = true
                this.row = null
                this.type = 'see'
                search(this.searchForm)
                    .then(({ list, total }) => {
                        this.searchForm.total = total
                        this.tableData = list
                    })
                    .finally(() => this.config.loading = false)
            },
            edit() {
                if (isEmpty(this.row)) return elError('请选择要编辑的弹幕')
                this.type = 'edit'
                this.editDialog = true
            },
            del() {
                if (isEmpty(this.row)) return elError('请选择要删除的弹幕')
                if (this.config.operating) return
                elConfirm(`确定删除该弹幕？`)
                    .then(() => {
                        this.config.operating = true
                        return del(this.row.id)
                    })
                    .then(() => this.success('删除成功'))
                    .finally(() => this.config.operating = false)
            },
            success(msg) {
                elSuccess(msg)
                this.editDialog = false
                this.search()
            }
        }
    }
</script>
