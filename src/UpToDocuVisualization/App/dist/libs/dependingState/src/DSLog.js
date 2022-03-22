function noop() {
}
function warnIfCalled(...data) {
    console.warn("warn log was not conditional. please add 'if (dsLog.enabled) { }' ");
    console.trace(data);
}
//
export class DSLogBase {
    constructor(name) {
        this.name = name;
        this.enabled = false;
        this.logEnabled = false;
        this.enableTiming = false;
        this.mode = "disabled";
        this.group = noop;
        this.groupEnd = noop;
        this.debug = noop;
        this.info = noop;
        this.log = noop;
        this.warn = noop;
        this.error = noop;
        this.trace = noop;
        this.flags = new Set();
    }
    initialize(mode) {
        // for now
        // dsLog.setSelfInGlobal();
        // dsLog.setMode("enabled");
        if (mode === undefined) {
            this.applyFromLocalStorage();
        }
        else {
            this.setMode(mode);
        }
        if (this.enabled) {
            dsLog.setSelfInGlobal();
        }
    }
    attach(storeManager) {
        this.storeManager = storeManager;
        // TODO: thinkof same as initialize
        if (this.enabled) {
            storeManager.enableTiming = this.enableTiming;
            storeManager.setSelfInGlobal();
        }
    }
    setDisabled() {
        // nop be quiet
        // console.debug(`${this.name} setDisabled`);
        this.enabled = false;
        this.logEnabled = false;
        this.mode = "disabled";
        this.group = noop;
        this.groupEnd = noop;
        this.debug = noop;
        this.info = noop;
        this.log = noop;
        this.warn = noop;
        this.error = noop;
        this.trace = noop;
        return this;
    }
    setEnabled() {
        if (this.mode == "enabled") {
            return this;
        }
        console.debug(`${this.name} setEnabled`);
        this.enabled = true;
        this.logEnabled = true;
        this.mode = "enabled";
        this.group = console.group;
        this.groupEnd = console.groupEnd;
        this.debug = console.debug;
        this.info = console.info;
        this.log = console.log;
        this.warn = console.warn;
        this.error = console.error;
        this.trace = console.trace;
        return this;
    }
    setWarnIfCalled() {
        if (this.mode == "WarnIfCalled") {
            return this;
        }
        console.debug(`${this.name} setWarnIfCalled`);
        this.enabled = true;
        this.logEnabled = false;
        this.mode = "WarnIfCalled";
        this.group = warnIfCalled;
        this.groupEnd = warnIfCalled;
        this.debug = warnIfCalled;
        this.info = warnIfCalled;
        this.log = warnIfCalled;
        this.warn = warnIfCalled;
        this.error = warnIfCalled;
        this.trace = warnIfCalled;
        return this;
    }
    setMode(mode) {
        if (mode === "enabled") {
            this.setEnabled();
        }
        else if (mode === "WarnIfCalled") {
            this.setWarnIfCalled();
        }
        else if (mode === "disabled") {
            this.setDisabled();
        }
        else {
            console.debug(`${this.name} setMode applyFromLocalStorage`);
            this.applyFromLocalStorage();
        }
    }
    saveToLocalStorage(key) {
        const data = {
            mode: this.mode
        };
        window.localStorage.setItem(key || this.name, JSON.stringify(data));
        return this;
    }
    applyFromLocalStorage(key) {
        const json = window.localStorage.getItem(key || this.name);
        if (json) {
            const data = JSON.parse(json);
            if (data) {
                if (typeof data.mode === "string") {
                    if (data.mode === "enabled") {
                        this.setEnabled();
                    }
                    else if (data.mode === "WarnIfCalled") {
                        this.setWarnIfCalled();
                    }
                    else {
                        this.setDisabled();
                    }
                }
            }
        }
        return this;
    }
    isEnabled(flag) {
        if (this.enabled) {
            if (this.flags.size === 0) {
                return true;
            }
            else {
                return this.flags.has(flag);
            }
        }
        return false;
    }
}
export function defaultConvertExtraArg(currentExtraArg) {
    if (currentExtraArg === undefined) {
        return "undefined";
    }
    if (currentExtraArg === null) {
        return "null";
    }
    if (typeof currentExtraArg === "string") {
        return currentExtraArg;
    }
    if (currentExtraArg.constructor && typeof currentExtraArg.constructor.name === "string") {
        return currentExtraArg.constructor.name;
    }
    return `${currentExtraArg}`;
}
function templateAMCE(log, currentApp, currentClass, currentMethod, currentExtraArg, //React.Component
message = undefined) {
    let effectiveArg = "";
    let calcedEffectiveArg = false;
    if (message === undefined) {
        message = "";
    }
    if (this.amceEnabled) {
        effectiveArg = this.convertArg(currentExtraArg);
        calcedEffectiveArg = true;
        if ((this.watchoutApp === undefined || this.watchoutApp === currentApp)
            && (this.watchoutClass === undefined || this.watchoutClass === currentClass)
            && (this.watchoutMethod === undefined || this.watchoutMethod === currentMethod)
            && (this.watchoutExtraArg === undefined || this.watchoutExtraArg === effectiveArg)) {
            this.watchoutHit++;
            if (dsLog.logEnabled) {
                console.warn(currentApp, currentClass, currentMethod, effectiveArg, this.watchoutHit, message);
            }
            if (this.watchoutStopAt === this.watchoutHit) {
                /*
                    the condition matched. have a look at the call stack.
                */
                debugger;
            }
            return;
        }
    }
    if (dsLog.logEnabled) {
        if (!calcedEffectiveArg) {
            effectiveArg = this.convertArg(currentExtraArg);
            calcedEffectiveArg = true;
        }
        log(currentApp, currentClass, currentMethod, effectiveArg, message);
    }
}
export class DSLogACME extends DSLogBase {
    constructor(name) {
        super(name);
        this.convertArg = defaultConvertExtraArg;
        this.amceEnabled = false;
        this.watchoutApp = undefined;
        this.watchoutClass = undefined;
        this.watchoutMethod = undefined;
        this.watchoutExtraArg = undefined;
        this.watchoutHit = 0;
        this.watchoutStopAt = undefined;
        this.debugACME = this.debug;
        this.infoACME = this.info;
        this.logACME = this.log;
        this.warnACME = this.warn;
        this.errorACME = this.error;
    }
    setDisabled() {
        super.setDisabled();
        if (this.amceEnabled) {
            this.bindACME();
        }
        else {
            this.enabled = false;
            this.debugACME = noop;
            this.infoACME = noop;
            this.logACME = noop;
            this.warnACME = noop;
            this.errorACME = noop;
        }
        return this;
    }
    setEnabled() {
        super.setEnabled();
        this.bindACME();
        return this;
    }
    setWarnIfCalled() {
        super.setWarnIfCalled();
        this.bindACME();
        return this;
    }
    setWatchout(watchoutApp = undefined, watchoutClass = undefined, watchoutMethod = undefined, watchoutExtraArg = undefined, watchoutStopAt = undefined) {
        this.watchoutApp = watchoutApp;
        this.watchoutClass = watchoutClass;
        this.watchoutMethod = watchoutMethod;
        this.watchoutExtraArg = watchoutExtraArg;
        this.watchoutStopAt = watchoutStopAt;
        const oldwatchoutEnabled = this.amceEnabled;
        this.amceEnabled = ((watchoutApp !== undefined)
            || (watchoutClass !== undefined)
            || (watchoutMethod !== undefined)
            || (watchoutExtraArg !== undefined));
        if (oldwatchoutEnabled != this.amceEnabled) {
            this.bindACME();
        }
        dsLog.info("DS setWatchout", watchoutApp, watchoutClass, watchoutMethod, watchoutExtraArg);
        this.enabled = this.amceEnabled || this.logEnabled;
        return this;
    }
    bindACME() {
        if (this.amceEnabled) {
            this.enabled = true;
            this.infoACME = templateAMCE.bind(this, console.info);
            this.debugACME = templateAMCE.bind(this, console.debug);
            this.logACME = templateAMCE.bind(this, console.log);
            this.warnACME = templateAMCE.bind(this, console.warn);
            this.errorACME = templateAMCE.bind(this, console.error);
        }
        else {
            this.debugACME = this.debug;
            this.infoACME = this.info;
            this.logACME = this.log;
            this.warnACME = this.warn;
            this.errorACME = this.error;
            this.enabled = this.logEnabled;
        }
    }
    clearFromLocalStorage(key) {
        if (!key) {
            key = this.name;
        }
        window.localStorage.removeItem(key);
        return this;
    }
    saveToLocalStorage(key) {
        const data = (this.amceEnabled) ? {
            mode: this.mode,
            watchoutEnabled: this.amceEnabled,
            watchoutApp: this.watchoutApp,
            watchoutClass: this.watchoutClass,
            watchoutMethod: this.watchoutMethod,
            watchoutExtraArg: this.watchoutExtraArg,
            watchoutStopAt: this.watchoutStopAt
        } : {
            mode: this.mode
        };
        window.localStorage.setItem(key || this.name, JSON.stringify(data));
        console.info(`window.localStorage.setItem('${(key || this.name)}', '${JSON.stringify(data)}');`);
        return this;
    }
    applyFromLocalStorage(key) {
        const json = window.localStorage.getItem(key || this.name);
        if (json) {
            const data = JSON.parse(json);
            if (data) {
                if (typeof data.mode === "string") {
                    if (data.mode === "enabled") {
                        this.setEnabled();
                    }
                    else if (data.mode === "WarnIfCalled") {
                        this.setWarnIfCalled();
                    }
                    else {
                        this.setDisabled();
                    }
                }
                if (data.watchoutEnabled === true) {
                    console.info("DS DSLogApp applyFromLocalStorage watchoutEnabled");
                    this.setWatchout((typeof data.watchoutApp === "string") ? data.watchoutApp : undefined, (typeof data.watchoutClass === "string") ? data.watchoutClass : undefined, (typeof data.watchoutMethod === "string") ? data.watchoutMethod : undefined, (typeof data.watchoutExtraArg === "string") ? data.watchoutExtraArg : undefined, (typeof data.watchoutStopAt === "number") ? data.watchoutStopAt : undefined);
                }
                else {
                    this.amceEnabled = false;
                    this.enabled = this.logEnabled;
                }
            }
        }
        return this;
    }
}
export class DSLog extends DSLogACME {
    constructor(name) {
        super(name || "dsLog");
    }
    setSelfInGlobal() {
        if (typeof window !== "undefined") {
            window.dsLog = this;
        }
    }
}
export const dsLog = new DSLog("dsLog");
