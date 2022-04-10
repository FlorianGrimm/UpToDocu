import {
    ConfigurationDSEntityValueStore,
    IDSObjectStateValue
} from "./types";

import { DSMapStore } from "./DSMapStore";
import { DSStateValue } from "./DSStateValue";

export class DSEntityStore<
    Key,
    Value,
    StoreName extends string = string
    > extends DSMapStore<Key, Value, StoreName>{
    constructor(
        storeName: StoreName,
        configuration?: ConfigurationDSEntityValueStore<Key, Value>
    ) {
        super(storeName, configuration);
    }

    public set(value: Value): IDSObjectStateValue<Value> {
        const getKey = (this.configuration as ConfigurationDSEntityValueStore<Key, Value>).getKey;
        const create = (this.configuration as ConfigurationDSEntityValueStore<Key, Value>).create;
        if (create !== undefined) {
            const result = create(value);
            const key = getKey(value);
            this.attach(key, result);
            return result;
        } else {
            const result = new DSStateValue<Value>(value);
            const key = getKey(value);
            this.attach(key, result);
            return result;
        }
    }

    public setMany(values: Value[]) {
        values.forEach(this.set, this);
    }

}