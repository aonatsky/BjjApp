const path = require('path');
const webpack = require('webpack');
const merge = require('webpack-merge');
const CheckerPlugin = require('awesome-typescript-loader').CheckerPlugin;
const ExtractTextPlugin = require('extract-text-webpack-plugin');
const extractTextPlugin = new ExtractTextPlugin('[name].css');
const UglifyJsPlugin = require('uglifyjs-webpack-plugin');

module.exports = env => {
  // Configuration in common to both client-side and server-side bundles
  const isDevBuild = !(env && env.prod);
  const sharedConfig = {
    stats: { modules: false },
    context: __dirname,
    resolve: { extensions: ['.js', '.ts', '.css', '.scss'] },
    output: {
      filename: '[name].js',
      publicPath: '/dist/' // Webpack dev middleware, if enabled, handles requests for this URL prefix
    },
    module: {
      rules: [
        {
          test: /\.ts$/,
          include: /ClientApp/,
          use: ['awesome-typescript-loader?silent=true', 'angular2-template-loader']
        },
        { test: /\.html$/, use: 'html-loader?minimize=false' },
        {
          test: /\.(css|scss)$/,
          use: ['to-string-loader'].concat(extractTextPlugin.extract({ use: ['css-loader', 'sass-loader'] }))
        },
        { test: /\.(png|jpg|jpeg|gif|svg)$/, use: 'url-loader?limit=25000' },
        {
          test: /\.(json)$/,
          use: [
            {
              loader: 'file-loader',
              options: {}
            }
          ]
        }
      ]
    },
    plugins: [new CheckerPlugin(), extractTextPlugin]
  };

  // Configuration for client-side bundle suitable for running in browsers
  const clientBundleOutputDir = './wwwroot/dist';
  const clientBundleConfig = merge(sharedConfig, {
    entry: { 'main-client': './ClientApp/boot-client.ts' },
    output: { path: path.join(__dirname, clientBundleOutputDir) },
    plugins: [
      new webpack.DllReferencePlugin({
        context: __dirname,
        manifest: require('./wwwroot/dist/vendor-manifest.json')
      })
    ].concat(
      isDevBuild
        ? [
            // Plugins that apply in development builds only
            new webpack.SourceMapDevToolPlugin({
              filename: '[file].map', // Remove this line if you prefer inline source maps
              moduleFilenameTemplate: path.relative(clientBundleOutputDir, '[resourcePath]') // Point sourcemap entries to the original file locations on disk
            })
          ]
        : [
            new UglifyJsPlugin()
            // Plugins that apply in production builds only
            //new webpack.optimize.UglifyJsPlugin()
          ]
    )
  });

  return [clientBundleConfig];
};
