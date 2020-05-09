const { resolve } = require("path");
const r = (path) => resolve(__dirname, path);

module.exports = {
  configureWebpack: {
    resolve: {
      alias: {
        "@img": r("img"),
      },
    },
  },
  title: "弹幕服务器文档",
  description: "Danmu.Server 弹幕服务器文档",
  theme: "antdocs",
  base: "/",
  head: [["link", { rel: "icon", href: "/favicon.png" }]],
  plugins: [
    ["sitemap", { hostname: "https://dandoc.u2sb.top" }],
    ["@vuepress/google-analytics", { ga: "UA-113200574-2" }],
    ["@vuepress/last-updated"],
    ["@vuepress/active-header-links"],
    ["@vuepress/nprogress"],
    [
      "@vuepress/medium-zoom",
      {
        selector: ".theme-antdocs-content :not(a) > img",
        options: {
          margin: 16,
          background: "#2B312C",
          scrollOffset: 60,
        },
      },
    ],
    ["@vuepress/search", { searchMaxSuggestions: 5 }],
    ["vuepress-plugin-pangu"],
    [
      "@vssue/vuepress-plugin-vssue",
      {
        platform: "github-v4",
        owner: "MonoLogueChi",
        repo: "vssue",
        clientId: "b26880b4ce394f432a26",
        clientSecret: "1460cd44beaa179927701ebc782cf7540eae811b",
      },
    ],
  ],
  markdown: {
    lineNumbers: true,
    extendMarkdown: (md) => {
      md.set({ breaks: true });
      md.use(require("markdown-it-sub"));
      md.use(require("markdown-it-sup"));
      md.use(require("markdown-it-footnote"));
      md.use(require("markdown-it-task-lists"));
      md.use(require("markdown-it-attrs"), {});
      md.use(require("markdown-it-imsize"));
    },
  },

  themeConfig: {
    ads: {
      style: 3,
      title: "赞助商",
      btnText: "成为赞助商",
      msgTitle: "成为赞助商",
      msgText:
        "如果您有品牌推广、活动推广、招聘推广、社区合作等需求，欢迎联系我们，成为赞助商。您的广告将出现在 AndDocs 文档侧边栏等页面。",
      msgOkText: "确定",
    },
    backToTop: true,
    logo: "img/logo_white_s.png",
    sidebarDepth: 2,
    smoothScroll: true,
    nav: [
      {
        text: "弹幕服务器",
        link: "/danmu/",
      },
      {
        text: "直播弹幕服务器",
        link: "/live/",
      },
      {
        text: "其他相关文档",
        link: "/other/",
      },
    ],
    sidebar: {
      "/danmu/": ["make", "install", "admin", "api"],
      "/live/": ["install", "api"],
      "/other/": ["pgsql", "otherapi", "fqa", "sponsors"],
    },
    repo: "MonoLogueChi/Danmu.Server",
    repoLabel: "GitHub",
    docsDir: "docs",
    docsBranch: "doc-source",
    editLinks: true,
    editLinkText: "在 GitHub 上编辑此页",
  },
};
