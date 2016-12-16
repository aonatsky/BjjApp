NodeJS installation flow:
1. Install NodeJS from https://nodejs.org/en/download/
2. Node comes with npm installed so you should have a version of npm. However, npm gets updated more frequently than Node does, so you'll want to make sure it's the latest version or install the latest version by the following command:
npm install npm@latest -g

Dev environment setup flow:
1. run "npm install.bat" with administrator rights to install packages from "package.json" file
2. run "npm start.bat" with administrator rights to launch the application, application is available by the following link: http://localhost:3000/ (port value can be configured '\config\webpack.dev.js' )
Upon any code change, changes are automatically detected by running npm process(started by "npm start.bat") and applied on dev environment. 

Application binaries preparation flow:
1. run "npm build.bat" with administrator rights to build published version of application to "dist" folder
