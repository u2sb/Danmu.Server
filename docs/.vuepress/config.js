const { resolve } = require("path");
const { load } = require("yaml-loader");
const merge = require("lodash.merge");
const r = path => resolve(__dirname, path);
const config = load(r("_config.yml"));

const configureWebpack = {
  resolve: {
    alias: {
      "@img": r("img")
    }
  }
};

const chainWebpack = (config, isServer) => {
  config.module
    .rule("vue")
    .uses.store.get("vue-loader")
    .store.get("options").transformAssetUrls = {
    "silentbox-single": "src"
  };
};

const markdown = {
  lineNumbers: true,
  extendMarkdown: md => {
    md.set({ breaks: true });
    md.use(require("markdown-it-sub"));
    md.use(require("markdown-it-sup"));
    md.use(require("markdown-it-footnote"));
    md.use(require("markdown-it-task-lists"));
    md.use(require("markdown-it-attrs"), {});
  }
};

const plugins = [
  ["sitemap", { hostname: "https://doc.video.xwhite.studio" }],
  ["@vuepress/google-analytics", { ga: "UA-113200574-2" }],
  ["@vuepress/last-updated"],
  ["@vuepress/active-header-links"],
  ["@vuepress/back-to-top"],
  ["@vuepress/nprogress"],
  [
    "@vuepress/medium-zoom",
    {
      options: {
        margin: 16,
        background: "#2B312C",
        scrollOffset: 100
      }
    }
  ],
  ["@vuepress/search", { searchMaxSuggestions: 5 }],
  ["vuepress-plugin-pangu"]
];

module.exports = merge(config, {
  configureWebpack,
  chainWebpack,
  markdown,
  plugins
});
