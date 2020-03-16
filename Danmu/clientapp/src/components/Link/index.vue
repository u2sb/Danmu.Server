<script type="text/jsx">
    import { isExternal } from '@/utils/validate'

    export default {
        name: 'Link',
        functional: true,
        render(h, context) {
            const { to, className } = context.props
            if (typeof to === 'string' && isExternal(to)) return (
                <a class={className} href={to} target="_blank" rel="nofollow">
                    {context.children}
                </a>
            )
            return (
                <router-link class={className} to={to}>
                    {context.children}
                </router-link>
            )
        }
    }
</script>
