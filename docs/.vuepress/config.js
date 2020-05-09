const { resolve } = require("path");
const { load } = require("yaml-loader");
const merge = require("lodash.merge");
const r = (path) => resolve(__dirname, path);
const config = load(r("_config.yml"));

const configureWebpack = {
  resolve: {
    alias: {
      "@img": r("img"),
    },
  },
};

const markdown = {
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
};

const plugins = [
  ["sitemap", { hostname: "https://dandoc.u2sb.top" }],
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
];

module.exports = merge(config, {
  configureWebpack,
  markdown,
  plugins,
});
