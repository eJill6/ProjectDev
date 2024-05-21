const playTypeRadioContext = require.context('', true, /\w+\.(vue)$/);

const playTypeRadioComponents = {} as { [k: string]: any };

const getComponentName = (gameTypeName: string, playTypeRadioName: string) => `${gameTypeName}_${playTypeRadioName}`;

playTypeRadioContext.keys().forEach(fileName => {
    let componentConfig = playTypeRadioContext(fileName);
    let fileNameParts = fileName.split('/');            
    let playTypeRadioName = fileNameParts[1].replace(/\.\w+$/, '');    
    let componentOptions = componentConfig.default;  
    playTypeRadioComponents[playTypeRadioName] = componentOptions;
});
  
export { playTypeRadioComponents, getComponentName };