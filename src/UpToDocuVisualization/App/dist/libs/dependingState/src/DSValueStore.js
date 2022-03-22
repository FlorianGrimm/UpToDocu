//IDSStoreManagerInternal 
import { dsLog } from './DSLog';
// State Value extends IDSStateValue<Value> = (Value extends IDSStateValue<Value> ? Value : IDSStateValue<Value>),
export class DSValueStore {
    constructor(storeName, configuration) {
        this.storeName = storeName;
        this.storeManager = undefined;
        this.mapEventHandlers = new Map();
        this._isDirty = false;
        this.stateVersion = 1;
        this.isProcessDirtyConfigured = false;
        this.triggerScheduled = false;
        this.viewStateVersion = 0;
        if (configuration === undefined) {
            this.configuration = {};
        }
        else {
            this.configuration = { ...configuration };
        }
    }
    get isDirty() {
        return this._isDirty;
    }
    /**
     * binds the events/actions from the storeBuilder to this valueStore
     * @param storeBuilder the storeBuilder to bind
     */
    setStoreBuilder(storeBuilder) {
        if (this.storeBuilder !== undefined) {
            throw new Error(`DS storeBuilder is already set ${this.storeName}`);
        }
        this.storeBuilder = storeBuilder;
        storeBuilder.bindValueStore(this);
    }
    /**
     * call all listenDirtyValue, listenCleanedUp, listenCleanedUpRelated and listenEvent.
     */
    initializeStore() {
        this.stateVersion = this.storeManager.getNextStateVersion(0);
        if (this.configuration.initializeStore !== undefined) {
            this.configuration.initializeStore();
        }
    }
    /**
     * called after initializeStore
     */
    initializeRegisteredEvents() {
        this.isProcessDirtyConfigured = this.hasProcessDirtyConfigured();
    }
    /**
     * called after initializeRegisteredEvents
     */
    validateRegisteredEvents() {
        if (this.arrCleanedUpRelated !== undefined) {
            if (!this.isProcessDirtyConfigured) {
                const relatedStoreNames = this.arrCleanedUpRelated.map(i => i.valueStore.storeName).join(", ");
                throw new Error(`${this.storeName}.processDirty (method or config) is not defined, but listenCleanedUpRelated was called from ${relatedStoreNames}.`);
            }
        }
    }
    /**
     * called after initializeStore (and after initializeRegisteredEvents)
     */
    initializeBoot() {
        if (this.configuration.initializeBoot !== undefined) {
            this.configuration.initializeBoot();
        }
    }
    hasProcessDirtyConfigured() {
        if (this.configuration.processDirty !== undefined) {
            return true;
        }
        if (!(this.processDirty === DSValueStore.prototype.processDirty)) {
            return true;
        }
        return false;
    }
    /**
     * gets all entities
     */
    getEntities() {
        return [];
    }
    getNextStateVersion(stateVersion) {
        if (this.storeManager === undefined) {
            return (this.stateVersion = (Math.max(this.stateVersion, stateVersion) + 1));
        }
        else {
            return (this.stateVersion = this.storeManager.getNextStateVersion(stateVersion));
        }
    }
    emitEvent(eventType, payload) {
        const event = {
            storeName: this.storeName,
            event: eventType,
            payload: payload
        };
        let arrEventHandlers = this.mapEventHandlers.get(event.event);
        if ((arrEventHandlers === undefined) || (arrEventHandlers?.length === 0)) {
            if ((this.storeManager !== undefined) && this.storeManager.warnUnlistenEvents) {
                if ((event.event === "attach") || (event.event === "detach") || (event.event === "value")) {
                }
                else {
                    dsLog.warnACME("DS", "DSValueStore", "emitEvent", `${this.storeName}/${event.event}`, "No event registered for listening");
                }
            }
        }
        else if (this.storeManager === undefined) {
            dsLog.warnACME("DS", "DSValueStore", "emitEvent", `${this.storeName}/${event.event}`, "this.storeManager is undefined");
            return this.processEvent(event);
        }
        else {
            return this.storeManager.emitEvent(event);
        }
    }
    /**
     * returns if any eventhandler is registered for this event
     * @param event the eventname
     */
    hasEventHandlersFor(event) {
        const arrEventHandlers = this.mapEventHandlers.get(event);
        if (arrEventHandlers === undefined) {
            return false;
        }
        else {
            return (arrEventHandlers.length > 0);
        }
    }
    listenEvent(msg, event, callback) {
        if (this.storeManager !== undefined) {
            if (this.storeManager.storeManagerState == 0) {
                throw new Error(`storeManagerState=0 has an unexpected value. Did you call all stores.attach?`);
            }
            else if (this.storeManager.storeManagerState == 1) {
                throw new Error(`storeManagerState=1 has an unexpected value. Did you call within initializeStore()?`);
            }
            else if (this.storeManager.storeManagerState == 2) {
                // OK
            }
            else if (this.storeManager.storeManagerState == 3) {
                throw new Error(`storeManagerState=1 has an unexpected value. Did you call within initializeStore()?`);
            }
            else if (this.storeManager.storeManagerState == 4) {
                throw new Error(`storeManagerState=1 has an unexpected value. Did you call within initializeStore()?`);
            }
            else {
                throw new Error(`storeManagerState=${this.storeManager.storeManagerState} has an unexpected value;`);
            }
        }
        let arrEventHandlers = this.mapEventHandlers.get(event);
        if (arrEventHandlers === undefined) {
            this.mapEventHandlers.set(event, [{ msg: msg, handler: callback }]);
        }
        else {
            this.mapEventHandlers.set(event, arrEventHandlers.concat([{ msg: msg, handler: callback }]));
        }
        this.storeManager?.resetRegisteredEvents();
        return this.unlistenEvent.bind(this, event, callback);
    }
    unlistenEvent(event, callback) {
        let arrEventHandlers = this.mapEventHandlers.get(event);
        if (arrEventHandlers === undefined) {
            // should not be
        }
        else {
            arrEventHandlers = arrEventHandlers.filter(cb => cb.handler !== callback);
            if (arrEventHandlers.length === 0) {
                this.mapEventHandlers.delete(event);
            }
            else {
                this.mapEventHandlers.set(event, arrEventHandlers);
            }
            this.storeManager?.resetRegisteredEvents();
        }
    }
    /**
     * internal
     * @param event
     */
    processEvent(event) {
        let r;
        let result;
        let arrEventHandlers = this.mapEventHandlers.get(event.event);
        if (arrEventHandlers === undefined) {
            // nobody is listening
            if (dsLog.enabled) {
                dsLog.infoACME("DS", "DSValueStore", "processEvent", `${event.storeName}/${event.event}`, "/nobody is listening");
            }
        }
        else {
            for (const { msg, handler: callback } of arrEventHandlers) {
                if (dsLog.enabled) {
                    dsLog.infoACME("DS", "DSValueStore", "processEvent", `${event.storeName}/${event.event}`, `/with ${msg}`);
                }
                if (r === undefined) {
                    try {
                        r = callback(event);
                    }
                    catch (reason) {
                        debugger;
                        dsLog.error(msg, reason);
                    }
                }
                else {
                    r = r.catch((reason) => {
                        debugger;
                        dsLog.error(msg, reason);
                    }).then(() => { return callback(event); });
                }
            }
        }
        if (r == undefined) {
            return;
        }
        else {
            return r.catch((reason) => {
                debugger;
                dsLog.error(reason);
            });
        }
    }
    /**
     * should be called after a value change - or willbe called from DSPropertiesChanged.valueChangedIfNeeded().
     * calls all callbacks - registed with listenDirtyValue - which can call setDirty if a relevant property was changed.
     * @param stateValue
     * @param properties
     */
    emitValueChanged(msg, stateValue, properties) {
        if ((this.arrValueChangedHandler !== undefined) || (this.isProcessDirtyConfigured)) {
            if (dsLog.isEnabled("emitValueChanged")) {
                dsLog.infoACME("DS", "DSValueStore", "emitValueChanged", this.storeName);
            }
            if ((this.arrValueChangedHandler !== undefined)) {
                for (const valueChangedHandler of this.arrValueChangedHandler) {
                    if (dsLog.enabled) {
                        dsLog.infoACME("DS", "DSValueStore", "emitValueChanged", valueChangedHandler.msg, "/dirtyHandler");
                    }
                    valueChangedHandler.handler(stateValue, properties);
                }
            }
            if (this.isProcessDirtyConfigured) {
                this.setDirty(msg ?? "emitValueChanged");
            }
        }
    }
    /**
     * register a callback that is called from emitDirtyValue.
     * @param msg the log message is logged before the callback is invoked.
     * @param callback the callback that will be called
     */
    listenValueChanged(msg, callback) {
        if (this.arrValueChangedHandler === undefined) {
            this.arrValueChangedHandler = [{ msg: msg, handler: callback }];
        }
        else {
            this.arrValueChangedHandler.push({ msg: msg, handler: callback });
        }
        return this.unlistenValueChanged.bind(this, callback);
    }
    /**
     * unregister the callback
     * @param callback the callback to unregister
     */
    unlistenValueChanged(callback) {
        if (this.arrValueChangedHandler !== undefined) {
            this.arrValueChangedHandler = this.arrValueChangedHandler.filter((cb) => cb.handler !== callback);
            if (this.arrValueChangedHandler.length === 0) {
                this.arrValueChangedHandler = undefined;
                // this.storeManager?.resetRegisteredEvents();
            }
        }
    }
    /**
     * set the isDirty flag and DSStoreManager.process will call processDirty
     * @param msg the message is logged if the store was not dirty
     */
    setDirty(msg) {
        if (this._isDirty) {
            return;
        }
        this._isDirty = true;
        if (this.storeManager === undefined) {
            dsLog.warnACME("DS", "DSValueStore", "setDirty", this.storeName, "storeManager is not set.");
        }
        else {
            this.storeManager.isDirty = true;
            dsLog.infoACME("DS", "DSValueStore", "setDirty", this.storeName, msg);
        }
    }
    /**
     *  DSStoreManager.process call this if setDirty was called before
     *  @returns true then emitCleanUp will be called
     */
    processDirty() {
        if (this.configuration.processDirty !== undefined) {
            return this.configuration.processDirty.apply(this);
        }
        else {
            return false;
        }
    }
    /**
     * called after processDirty()
     */
    postProcessDirty(processDirtyResult) {
        this._isDirty = false;
        if (processDirtyResult) {
            this.emitCleanedUp();
        }
        else {
            //
        }
    }
    /**
     * would be called if processDirty returns true
     */
    emitCleanedUp() {
        if (this.arrCleanedUpRelated !== undefined) {
            for (const cleanedUpRelated of this.arrCleanedUpRelated) {
                var relatedValueStore = cleanedUpRelated.valueStore;
                relatedValueStore.setDirty(cleanedUpRelated.msg);
            }
        }
        if (this.arrCleanedUpHandler !== undefined) {
            for (const cleanedUpHandler of this.arrCleanedUpHandler) {
                cleanedUpHandler.handler(this);
            }
        }
    }
    /**
     * register a callback that is (directly) invoked by emitDirty
     * @param msg
     * @param callback
     */
    listenCleanedUp(msg, callback) {
        const cleanedUpHandler = { msg: msg, handler: callback };
        if (this.arrCleanedUpHandler === undefined) {
            this.arrCleanedUpHandler = [cleanedUpHandler];
        }
        else {
            this.arrCleanedUpHandler.push(cleanedUpHandler);
        }
        return this.unlistenCleanedUp.bind(this, callback);
    }
    /**
     * unregister a callback
     * @param callback
     */
    unlistenCleanedUp(callback) {
        if (this.arrCleanedUpHandler !== undefined) {
            this.arrCleanedUpHandler = this.arrCleanedUpHandler.filter((item) => (item.handler !== callback));
            if (this.arrCleanedUpHandler.length === 0) {
                this.arrCleanedUpHandler = undefined;
            }
        }
    }
    /**
      * if this store gets cleanedup (processDirty returns true) the relatedValueStore gets dirty.
      * @param msg
      * @param relatedValueStore
      */
    listenCleanedUpRelated(msg, relatedValueStore) {
        if (this.arrCleanedUpRelated === undefined) {
            this.arrCleanedUpRelated = [];
        }
        const index = this.arrCleanedUpRelated.findIndex((item) => (item.valueStore === relatedValueStore));
        if (index < 0) {
            this.arrCleanedUpRelated = (this.arrCleanedUpRelated || []).concat([{ msg: msg, valueStore: relatedValueStore }]);
            return (() => { this.unlistenCleanedUpRelated(relatedValueStore); });
        }
        else {
            return (() => { });
        }
    }
    /**
     * unregister the relatedValueStore
     * @param relatedValueStore
     */
    unlistenCleanedUpRelated(relatedValueStore) {
        if (this.arrCleanedUpRelated !== undefined) {
            this.arrCleanedUpRelated = this.arrCleanedUpRelated.filter((item) => (item.valueStore !== relatedValueStore));
            if (this.arrCleanedUpRelated.length === 0) {
                this.arrCleanedUpRelated = undefined;
            }
        }
    }
    emitUIUpdate(uiStateValue) {
        if (this.storeManager === undefined) {
            uiStateValue.triggerUIUpdate(this.stateVersion);
        }
        else {
            if ((this._ViewProps !== undefined) && !this.triggerUIUpdate) {
                this.storeManager.emitUIUpdate(this);
            }
            this.storeManager.emitUIUpdate(uiStateValue);
        }
    }
    triggerUIUpdate(stateVersion) {
        this.triggerScheduled = false;
        // const stateVersion = this.stateValue.stateVersion;
        // if (this.component === undefined) {
        //     //
        // } else {
        //     if (stateVersion === this.viewStateVersion) {
        //         //
        //         dsLog.info(`DSUIStateValue skip update same stateVersion: ${stateVersion}`)
        //     } else {
        this.viewStateVersion = stateVersion;
        const enabled = (dsLog.enabled && dsLog.isEnabled("triggerUIUpdate"));
        if (this.arrComponentStateVersionName === undefined) {
            //
        }
        else if (Array.isArray(this.arrComponentStateVersionName)) {
            for (const componentStateVersionName of this.arrComponentStateVersionName) {
                componentStateVersionName.component.setState({ [componentStateVersionName.stateVersionName]: stateVersion });
                if (enabled) {
                    dsLog.infoACME("DS", "DSUIStateValue", "triggerUIUpdate", dsLog.convertArg(componentStateVersionName));
                }
            }
        }
        else {
            this.arrComponentStateVersionName.component.setState({ [this.arrComponentStateVersionName.stateVersionName]: stateVersion });
            if (enabled) {
                dsLog.infoACME("DS", "DSUIStateValue", "triggerUIUpdate", dsLog.convertArg(this.arrComponentStateVersionName));
            }
        }
        //     }
        // }
    }
    getViewProps() {
        if (this._ViewProps === undefined) {
            const fnGetRenderProps = (() => {
                return this;
            });
            const fnWireStateVersion = ((component, stateVersionName) => {
                const csvn = { component: component, stateVersionName: stateVersionName ?? "stateVersion" };
                if (this.arrComponentStateVersionName === undefined) {
                    this.arrComponentStateVersionName = csvn;
                }
                else if (Array.isArray(this.arrComponentStateVersionName)) {
                    this.arrComponentStateVersionName.push(csvn);
                }
                else {
                    this.arrComponentStateVersionName = [this.arrComponentStateVersionName, csvn];
                }
                return this.stateVersion;
            });
            const fnUnwireStateVersion = ((component) => {
                if (this.arrComponentStateVersionName === undefined) {
                    // done
                }
                else if (Array.isArray(this.arrComponentStateVersionName)) {
                    for (let idx = 0; idx < this.arrComponentStateVersionName.length; idx++) {
                        if (this.arrComponentStateVersionName[idx].component === component) {
                            this.arrComponentStateVersionName.splice(idx, 1);
                            if (this.arrComponentStateVersionName.length === 1) {
                                this.arrComponentStateVersionName = this.arrComponentStateVersionName[0];
                            }
                            return;
                        }
                    }
                }
                else {
                    if (this.arrComponentStateVersionName.component === component) {
                        this.arrComponentStateVersionName = undefined;
                    }
                }
            });
            const fnGetStateVersion = (() => {
                return this.stateVersion;
            });
            //
            this._ViewProps = {
                getRenderProps: fnGetRenderProps,
                wireStateVersion: fnWireStateVersion,
                unwireStateVersion: fnUnwireStateVersion,
                getStateVersion: fnGetStateVersion,
            };
        }
        return this._ViewProps;
    }
}
