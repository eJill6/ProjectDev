const w = window as any;

const toTokenPath = (path: string): string => w.toTokenPath(path);

const openUrl = (logonMode: Number, url: string, win: object | null) =>
  w.openUrl(logonMode, url, win);

const openUrlBlank=(url:string)=>
  w.open(url,'_blank');

  
export default {
  toTokenPath,
  openUrl,
  openUrlBlank
};