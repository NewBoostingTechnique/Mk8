import { defineConfig } from 'vite'
import dynamicImport from 'vite-plugin-dynamic-import'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
  build: {
    rollupOptions: {
      input: {
        app: './App/Index.html'
      }
    }
  },
  plugins: [
    dynamicImport(),
    react()
  ],
})
