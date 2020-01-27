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
  config.module.rule('vue').uses.store.get('vue-loader').store.get('options').transformAssetUrls = {
    "silentbox-single": 'src'
  };
}

const markdown = {
  lineNumbers: true,
  extendMarkdown: md => {
    md.set({ breaks: true });
    md.use(require("markdown-it-sub"));
    md.use(require("markdown-it-sup"));
    md.use(require("markdown-it-footnote"));
    md.use(require("markdown-it-task-lists"));
    md.use(require("markdown-it-attrs"), {});

    md.renderer.rules.image = function(tokens, idx, options, env, self) {
      var token = tokens[idx],
        aIndex = token.attrIndex("src"),
        src = token.attrs[aIndex][1]

      return (
        '<silentbox-single src="' + src + '" description="">\n' +
        '  <img src="' + src + '">\n' +
        "</silentbox-single>\n"
      );
    };
  }
};

const plugins = [
  ["sitemap", { hostname: "https://doc.video.xwhite.studio" }],
  ["@vuepress/google-analytics", { ga: "UA-113200574-2" }],
  ["@vuepress/last-updated"],
  ["@vuepress/active-header-links"],
  ["@vuepress/back-to-top"],
  ["@vuepress/nprogress"],
  ["@vuepress/search", { searchMaxSuggestions: 5 }],
  ["vuepress-plugin-pangu"]
];

module.exports = merge(config, { configureWebpack, chainWebpack, markdown, plugins });
