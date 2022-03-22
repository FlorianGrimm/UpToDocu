export class DSUIBinder {
    constructor(component, props) {
        this.component = component;
        this.props = props;
        this.state = {
            stateVersion: props.getStateVersion()
        };
        this.arrUnwireStateVersion = [];
        this.add("stateVersion", props);
    }
    add(stateVersionName, nextProps) {
        if (this.state === undefined) {
            throw new Error(`add must be called before getState. ${this.component.constructor.name}.`);
        }
        if (this.arrUnwireStateVersion === undefined) {
            throw new Error(`add must be called before setComponentWillUnmount or getUnbinder.${this.component.constructor.name}.`);
        }
        //this.state![stateVersionName] = nextProps.getStateVersion();
        this.state[stateVersionName] = nextProps.wireStateVersion(this.component, stateVersionName);
        this.arrUnwireStateVersion.push(nextProps.unwireStateVersion);
        return this;
    }
    bindHandleAll() {
        const p = Object.getPrototypeOf(this.component);
        for (const key of Object.getOwnPropertyNames(p)) {
            if (key.startsWith("handle")) {
                // console.log("bindHandleAll", key);
                if (Object.prototype.hasOwnProperty.call(p, key)) {
                    const fn = p[key];
                    if (typeof fn === "function") {
                        // console.log("bindHandleAll", key);
                        this.component[key] = fn.bind(this.component);
                    }
                    else {
                        // console.log("bindHandleAll not", key);
                    }
                }
                else {
                    // console.log("bindHandleAll not", key);
                }
            }
        }
        return this;
    }
    bindHandle(fnName) {
        if (Array.isArray(fnName)) {
            for (const key in fnName) {
                if (Object.prototype.hasOwnProperty.call(this.component, key)) {
                    const fn = this.component[key];
                    if (typeof fn === "function") {
                        this.component[key] = fn.bind(this.component);
                    }
                    else {
                        throw new Error(`${key}`);
                    }
                }
            }
        }
        else {
            const fn = this.component[fnName];
            this.component[fnName] = fn.bind(this.component);
        }
        return this;
    }
    setComponentWillUnmount() {
        if (this.state === undefined) {
            throw new Error(`setComponentWillUnmount must be called before getState. ${this.component.constructor.name}.`);
        }
        if (this.arrUnwireStateVersion === undefined) {
            throw new Error(`setComponentWillUnmount or getUnbinder can be called only once.${this.component.constructor.name}.`);
        }
        const arrUnwireStateVersion = this.arrUnwireStateVersion;
        this.arrUnwireStateVersion = undefined;
        const prevComponentWillUnmount = this.component.componentWillUnmount;
        this.component.componentWillUnmount = componentWillUnmountTemplate.bind(undefined, this.component, prevComponentWillUnmount, arrUnwireStateVersion);
        return this;
    }
    getUnbinder() {
        if (this.state === undefined) {
            throw new Error(`getUnbinder must be called before getState.${this.component.constructor.name}.`);
        }
        if (this.arrUnwireStateVersion === undefined) {
            throw new Error(`setComponentWillUnmount or getUnbinder can be called only once.${this.component.constructor.name}.`);
        }
        const arrUnwireStateVersion = this.arrUnwireStateVersion;
        this.arrUnwireStateVersion = undefined;
        const prevComponentWillUnmount = this.component.componentWillUnmount;
        return componentWillUnmountTemplate.bind(undefined, this.component, prevComponentWillUnmount, arrUnwireStateVersion);
    }
    getState() {
        console.log("setComponentWillUnmount", this.component);
        if (this.state === undefined) {
            throw new Error(`getState cannot be called twice. ${this.component.constructor.name}.`);
        }
        if (this.arrUnwireStateVersion !== undefined) {
            throw new Error(`setComponentWillUnmount or getUnbinder must be called before getState.${this.component.constructor.name}.`);
        }
        const result = this.state;
        this.state = undefined;
        return result;
    }
}
export function bindUIComponent(component, props) {
    return new DSUIBinder(component, props);
}
function componentWillUnmountTemplate(component, prevComponentWillUnmount, arrUnwireStateVersion) {
    for (const unwireStateVersion of arrUnwireStateVersion) {
        unwireStateVersion(component);
    }
    if (prevComponentWillUnmount !== undefined) {
        prevComponentWillUnmount.call(component);
    }
}
