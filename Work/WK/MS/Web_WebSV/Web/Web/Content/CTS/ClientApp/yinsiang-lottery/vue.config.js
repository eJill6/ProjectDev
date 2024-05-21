module.exports = {
    publicPath: '/Content/CTS/ClientApp/dist/yinsiang-lottery', // js運行目錄
    outputDir: '../dist/yinsiang-lottery', // 建置js目錄
    filenameHashing: false, // 建置後的檔名不產生hash 
    productionSourceMap: false, // 建置後不產生map file 
    chainWebpack: config => { // 建置後不產生index.html 
        config.plugins.delete('html')
        config.plugins.delete('preload')
        config.plugins.delete('prefetch')
        config.module
            .rule('images')
            .set('parser', {
                dataUrlCondition: {
                maxSize: 0 
            }})
    },
    devServer: { // 熱重載配置 
        proxy: {
            './*': {
                ws: false,
                target: 'http://localhost:62340',
                changeOrigin: true
            }
        }
    }
};