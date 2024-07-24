import react from '@vitejs/plugin-react-swc'
import { defineConfig } from 'vite'
import dynamicImport from 'vite-plugin-dynamic-import'

export default defineConfig({
  build: {
    emptyOutDir: true,
    outDir: 'wwwroot',
    rollupOptions: {
      input: {
        app: './App/Index.html'
      },
      onwarn(warning, warn) {
        if (
          warning.code === "MODULE_LEVEL_DIRECTIVE" &&
          warning.message.includes(`"use client"`)
        ) {
          return;
        }
        warn(warning);
      },
    },
    target: "es2022"
  },
  esbuild: {
    target: "es2022"
  },
  optimizeDeps: {
    esbuildOptions: {
      target: "es2022",
    }
  },
  plugins: [
    dynamicImport(),
    react()
  ],
  root: '.'
})
