import {
    dsLog,
    DSMapStore,
    DSStoreManager,
} from "../index";

type VSA = {
    a: number;
}

type VSB = {
    b: number;
}
type VSAB = {
    a: number;
    b: number;
    cnt: number;
}

test('DSMapStore_process_implicit', () => {
    dsLog.initialize("disabled");
    const storeManager = new DSStoreManager();
    storeManager.warnEventsOutOfProcess = false;
    const valueStoreA = new DSMapStore<number, VSA>("a");
    storeManager.attach(valueStoreA);
    let unlisten = () => { };
    storeManager.initialize(() => {
        unlisten = valueStoreA.listenEventValue("test", (dsEvent) => {
            actHit = true;
            actA = dsEvent.payload.entity!.value.a;
        });
    });
    expect(valueStoreA.storeManager).toBe(storeManager);

    let actHit = false;
    let actA = 1;
    const valueA = valueStoreA.create(42, { a: actA });
    expect(valueA.store).toBe(valueStoreA);

    valueA.value = { a: 11 };
    expect(storeManager.events.length).toBe(0);

    expect(actHit).toBe(true);
    expect(actA).toBe(11);

    valueA.value = { a: 22 };
    expect(actA).toBe(22);

    unlisten();

    valueA.value = { a: 33 };
    expect(actA).toBe(22);
});


test('DSMapStore_process_explicit', () => {
    dsLog.initialize("disabled");
    const storeManager = new DSStoreManager();
    const valueStoreA = new DSMapStore<number, VSA>("a");
    storeManager.attach(valueStoreA);
    let unlisten = () => { };
    storeManager.initialize(() => {
        unlisten = valueStoreA.listenEventValue("test", (dsEvent) => {
            actHit = true;
            actA = dsEvent.payload.entity.value.a;
        });
    });
    expect(valueStoreA.storeManager).toBe(storeManager);

    let actHit = false;
    let actA = 1;
    const valueA = valueStoreA.create(42, { a: actA });
    expect(valueA.store).toBe(valueStoreA);

    storeManager.process("test 2 11", () => {
        valueA.value = { a: 2 };
        expect(storeManager.events.length).toBe(1);

        valueA.value = { a: 11 };
        expect(actA).toBe(1);
    });
    expect(actA).toBe(11);

    storeManager.process("test 22", () => {
        valueA.value = { a: 22 };
    });
    expect(actA).toBe(22);

    unlisten();

    valueA.value = { a: 33 };
    expect(actA).toBe(22);
});


test('DSMapStore_listen', async () => {
    dsLog.initialize("disabled");
    const storeManager = new DSStoreManager();

    const valueStoreA = new DSMapStore<number, VSA>("a");
    const valueStoreB = new DSMapStore<number, VSB>("b");
    const valueStoreAB = new DSMapStore<number, VSAB>("ab");
    storeManager.attach(valueStoreA).attach(valueStoreB).attach(valueStoreAB);
    storeManager.initialize(() => {
        valueStoreA.listenEventValue("testa", (dsEvent) => {
            valueAB.value.a = dsEvent.payload.entity!.value.a;
            valueAB.value.cnt++;
            valueAB.valueChanged("testa");
        });
        valueStoreB.listenEventValue("testb", (dsEvent) => {
            valueAB.value.b = dsEvent.payload.entity!.value.b;
            valueAB.value.cnt++;
            valueAB.valueChanged("testb");
        });
    });

    const valueA = valueStoreA.create(42, { a: 0 });
    const valueB = valueStoreB.create(42, { b: 0 });
    const valueAB = valueStoreAB.create(42, { a: 0, b: 0, cnt: 0 });


    valueA.value = { a: 1 };
    valueB.value = { b: 2 };

    expect(valueAB.value).toStrictEqual({ a: 1, b: 2, cnt: 2 });
});


test('DSMapStore_process_promise', async () => {
    dsLog.initialize("disabled");
    const storeManager = new DSStoreManager();

    const valueStoreA = new DSMapStore<number, VSA>("a");
    const valueStoreB = new DSMapStore<number, VSB>("b");
    const valueStoreAB = new DSMapStore<number, VSAB>("ab");
    storeManager.attach(valueStoreA).attach(valueStoreB).attach(valueStoreAB);
    storeManager.initialize(() => {
        valueStoreA.listenEventValue("testa", (dsEvent) => {
            valueAB.value.a = dsEvent.payload.entity!.value.a;
            valueAB.value.cnt = valueAB.value.cnt * 10 + 1;
            var result = new Promise((resolve) => {
                setTimeout(() => { valueAB.valueChanged("testa"); }, 200);
                resolve(undefined);
            });
            return result;
        });
        valueStoreB.listenEventValue("testb", (dsEvent) => {
            valueAB.value.b = dsEvent.payload.entity!.value.b;
            valueAB.value.cnt = valueAB.value.cnt * 100 + 2;
            var result = new Promise((resolve) => {
                setTimeout(() => { valueAB.valueChanged("testb"); }, 50);
                resolve(undefined);
            });
            return result;
        });
    });

    const valueA = valueStoreA.create(42, { a: 0 });
    const valueB = valueStoreB.create(42, { b: 0 });
    const valueAB = valueStoreAB.create(42, { a: 0, b: 0, cnt: 0 });

    await storeManager.process("test", () => {
        valueA.value = { a: 1 };
        valueB.value = { b: 2 };
    });

    expect(valueAB.value).toStrictEqual({ a: 1, b: 2, cnt: 100 * 1 + 2 });
});


test('DSMapStore copy', async () => {
    dsLog.initialize("disabled");
    const storeManager = new DSStoreManager();

    const valueStoreA = new DSMapStore<number, VSA>("a");
    const valueStoreB = new DSMapStore<number, VSB>("b");
    const valueStoreAB = new DSMapStore<number, VSAB>("ab");
    storeManager.attach(valueStoreA).attach(valueStoreB).attach(valueStoreAB);
    storeManager.initialize(() => {
        valueStoreA.listenEventAttach("test", (dsEvent) => {
            const key = dsEvent.payload.key!;
            const v = valueStoreB.entities.get(key)
            if (v === undefined) {
                valueStoreB.create(key, { b: dsEvent.payload.entity.value.a });
            } else {
                debugger;
                throw "unexpected map 179"
            }
        });

        valueStoreA.listenEventValue("test", (dsEvent) => {
            debugger;
            throw "unexpected map 184"
        })
    });

    const valueA1 = valueStoreA.create(42, { a: 1 });
    expect(valueStoreB.entities.size).toBe(1);

    const valueA2 = valueStoreA.create(21, { a: 2 });

    expect(valueStoreB.entities.size).toBe(2);
});