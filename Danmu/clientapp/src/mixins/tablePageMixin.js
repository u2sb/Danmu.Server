/*
* 列表页通用混入
* 引用者必须要有search方法
* */
import { mapState } from 'vuex'

const pcPaginationLayout = 'total, prev, pager, next, jumper'
const mobilePaginationLayout = 'prev, pager, next'

const mixin = {
    data() {
        return {
            searchForm: {
                page: 1,
                size: 15,
                total: 0
            },
            config: {
                loading: false,
                operating: false
            },
            tableData: [],
            row: null,
            type: 'see'
        }
    },
    computed: {
        ...mapState('app', {
            paginationLayout: state => state.device === 'pc' ? pcPaginationLayout : mobilePaginationLayout
        })
    },
    watch: {
        row(v) {
            !v && this.$refs.table && this.$refs.table.setCurrentRow()
        }
    },
    methods: {
        pageChange(v) {
            this.searchForm.page = v
            this.search()
        }
    },
    mounted() {
        this.search()
    }
}

export default mixin
