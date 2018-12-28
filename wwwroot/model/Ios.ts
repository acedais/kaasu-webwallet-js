export default class Ios {
	savePass (password: any) {
		if (window.webkit) {
			window.webkit.messageHandlers.action.postMessage({password:password});
		}
	}
}