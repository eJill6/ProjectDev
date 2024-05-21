const w = window as any;

const toTokenPath = (path: string): string => w.toTokenPath(path);

const openUrl = (logonMode: Number, url: string, win: object | null) =>
  w.openUrl(logonMode, url, win);
export default {
  toTokenPath,
  openUrl,
};
