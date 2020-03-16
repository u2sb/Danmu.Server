<script type="text/jsx">
    import AppLink from '@/components/Link'

    export default {
        name: 'Welcome',
        functional: true,
        render() {
            const links = [
                    { to: 'https://www.baidu.com', text: '文档' },
                    { to: '/login', text: '后台' }
                ],
                intros = [
                    {
                        img: '/img/img-people.svg',
                        intro: `<h3 class="slogan-title">我们相信</h3>
                               <h3 class="slogan-desc">人人都能<br>与全球互联网自由相连</h3>
                               <h3 class="slogan-desc">人人都能<br>创造更多价值</h3>`,
                        direction: 'ltr'
                    },
                    {
                        img: '/img/img-devices.svg',
                        intro: `<h3 class="slogan-title">简单 值得信赖</h3>
                               <h3 class="slogan-desc">我们的服务与诸多平台上的优秀应用程序兼容<br>我们与开发者一同进步 实时更新 只为提供最优秀的体验</h3>`,
                        direction: 'rtl'
                    }
                ]
            const nav = (
                <nav class="welcome-nav">
                    <a class="welcome-nav-logo">
                        <img src="/favicon.png"/>
                    </a>
                    <div class="nav-links">
                        {links.map(({ to, text }) => (
                            <AppLink class-name="nav-item" to={to}>{text}</AppLink>
                        ))}
                    </div>
                </nav>
            )
            const intro = intros.map(({ img, intro, direction }) => {
                const image = (
                    <el-col xs={24} sm={12}>
                        <img src={img} style="width: 100%"/>
                    </el-col>
                )
                const content = <el-col xs={24} sm={12} domPropsInnerHTML={intro}/>
                return (
                    <el-row class="welcome-page-banner">
                        {direction === 'ltr' ? [image, content] : [content, image]}
                    </el-row>
                )
            })
            intro.unshift(
                <h2 class="section-title">“ Danmu.Server，一个开源的弹幕服务器 ”</h2>
            )
            return (
                <div class="welcome-wrapper">
                    <div class="welcome-background"/>
                    <div class="welcome-main">
                        {[nav, intro]}
                        <div class="welcome-image"/>
                    </div>
                </div>
            )
        }
    }
</script>

<style lang="scss">
    .welcome-wrapper {
        min-height: 100%;
        width: 100%;
        background: #030415;

        .welcome-background {
            position: fixed;
            width: 100%;
            height: 800px;
            background: linear-gradient(0.12deg, rgba(3, 4, 21, .2) 0, rgba(69, 61, 180, 0.2) 61.84%, rgba(33, 59, 119, 0.2) 99.97%, rgba(3, 4, 21, 0.2) 100%);
        }

        .welcome-main {
            position: relative;
            z-index: 1;
            margin: 0 auto;
            max-width: 1100px;
        }

        .welcome-nav {
            display: flex;
            justify-content: space-between;
            align-items: center;
            min-height: 50px;
            padding-top: 20px;
            font-size: 14px;

            .welcome-nav-logo {
                height: 50px;
                margin-left: 1.5rem;

                img {
                    height: 100%;
                    vertical-align: middle;
                }
            }

            .nav-item {
                display: inline-block;
                line-height: 2rem;
                margin-right: 1.5rem;
                color: #777;

                &:hover {
                    color: #fff;
                }
            }
        }

        .welcome-page-banner {
            margin-top: 180px;
            margin-bottom: 120px;
            @media (max-width: 768px) {
                margin-top: 70px;
            }

            .el-col {
                padding-right: 15px;
                padding-left: 15px;
            }
        }

        .welcome-image {
            height: 800px;
            background: url(/img/map@3x.png) no-repeat center center;
            background-size: 100%;
            @media (max-width: 768px) {
                height: 400px;
            }
        }

        .section-title {
            color: #fff;
            font-size: 43px;
            font-weight: 400;
            text-align: center;
            margin-top: 180px;
            padding: 0 15px;
            @media (max-width: 768px) {
                margin-top: 70px;
            }
        }

        .slogan-title {
            color: #fff;
            font-size: 35px;
            font-weight: 400;
            line-height: 65px;
            margin-top: 60px;
        }

        .slogan-desc {
            color: rgba(255, 255, 255, .7);
            font-size: 21px;
            font-weight: 300;
            line-height: 35px;
            text-align: justify;
        }
    }
</style>
