import { defineConfig } from "vite";
import vue from "@vitejs/plugin-vue";
import legacy from "@vitejs/plugin-legacy";

// https://vitejs.dev/config/
export default defineConfig({
  base: "",
  build: {
    outDir: "../DanMu/DanMu/wwwroot/",
  },
  plugins: [
    vue(),
    legacy({
      targets: ["> 1%", "last 2 versions", "not dead", "not ie 11"],
    }),
  ],
  resolve: {
    alias: {
      "@": "/src",
    },
  },
  css: {
    preprocessorOptions: {
      less: {
        // modifyVars: generateModifyVars(),
        javascriptEnabled: true,
      },
    },
  },
});
