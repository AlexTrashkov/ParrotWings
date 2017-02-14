var webpack = require("webpack");
var path = require("path");
var HtmlWebpackPlugin = require('html-webpack-plugin');
var CopyWebpackPlugin = require('copy-webpack-plugin');

module.exports = {
	context: __dirname,
	entry: {
		app: './src/main',
		polyfill: './src/polyfill'
	},
	output: {
		filename: "[name].js",
		path: path.join(__dirname, "../ParrotWings.Server/ParrotWings.Server.Web/wwwroot")
	},
	resolve: {
		extensions: ['', '.ts', '.tsx', '.js', '.jsx', '.html']
	},
	module: {
		loaders: [
			{
				test: /\.ts$/,
				loader: 'awesome-typescript-loader'
			},
			{
				test: /\.html$/,
				loader: 'html-loader'
			},
			{
				test: /\.scss$/,
				loaders: ['raw-loader', 'sass-loader']
			},
			{
				test: /\.(jpe?g|png|gif|svg)$/i,
				loaders: [
					'file?hash=sha512&digest=hex&name=[hash].[ext]',
					'image-webpack?bypassOnDebug&optimizationLevel=7&interlaced=false'
				]
			}
		]
	},
	plugins: [
		new HtmlWebpackPlugin({
			template: './src/index.html'
		}),
		new CopyWebpackPlugin([
			{
				from: 'src/assets/i18n',
				to: 'i18n'
			}
		])
	]
}