let global : any = typeof window !== 'undefined' ? window : self;
global.config = {
    apiUrl: typeof window !== 'undefined' && window.location ? window.location.href.substr(0, window.location.href.lastIndexOf('/') + 1) + 'api/' : 'https://getkaasu.net/api/',
	mainnetExplorerUrl: "http://getkaasu.info",
	testnetExplorerUrl: "http://getkaasu.info/",
	testnet: false,
    coinUnitPlaces: 5,
    coinDisplayUnitPlaces: 5,
	txMinConfirms: 60,
	txCoinbaseMinConfirms: 160,
	addressPrefix: 111,
	integratedAddressPrefix: 0,
	addressPrefixTestnet: 0,
	integratedAddressPrefixTestnet: 0,
	subAddressPrefix: 0,
	subAddressPrefixTestnet: 0,
	feePerKB: new JSBigInt('3'), //for testnet its not used, as fee is dynamic.
	dustThreshold: new JSBigInt('3'),//used for choosing outputs/change - we decompose all the way down if the receiver wants now regardless of threshold
	defaultMixin: 0, // default value mixins
	idleTimeout: 30,
	idleWarningDuration: 20,
	coinSymbol: 'KSU',
	coinName: 'Kaasu',
	coinUriPrefix: 'kaasu:',
	avgBlockTime: 120,
	maxBlockNumber: 500000000,
	pageRange: 5 // transactions paging
};
