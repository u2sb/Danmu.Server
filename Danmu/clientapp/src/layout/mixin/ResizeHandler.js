const { body } = document
const WIDTH = 768

export default {
    watch: {
        $route() {
            if (this.device === 'mobile' && !this.collapseSidebar) {
                this.$store.commit('app/setCollapseSidebar', true)
            }
        }
    },
    methods: {
        $_isMobile() {
            const rect = body.getBoundingClientRect()
            return rect.width - 1 < WIDTH
        },
        $_resizeHandler() {
            if (!document.hidden) {
                const isMobile = this.$_isMobile()
                this.$store.commit('app/setDevice', isMobile ? 'mobile' : 'pc')
                if (isMobile) this.$store.commit('app/setCollapseSidebar', true)
            }
        }
    },
    mounted() {
        window.addEventListener('resize', this.$_resizeHandler)
        this.$_resizeHandler()
    },
    beforeDestroy() {
        window.removeEventListener('resize', this.$_resizeHandler)
    }
}
