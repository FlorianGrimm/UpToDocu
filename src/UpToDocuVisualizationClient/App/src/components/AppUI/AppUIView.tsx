import {
    bindUIComponent,
    DSUIProps,
    DSUIViewStateBase
} from "dependingState";

import React from "react";
import NavigatorView from "~/components/Navigator/NavigatorView";
import { getAppStoreManager } from "~/singletonAppStoreManager";

import type { AppUIValue } from "./AppUIValue";

import "./AppUIView.css";

type AppViewProps = {
} & DSUIProps<AppUIValue>;

type AppViewState = {
} & DSUIViewStateBase;

/**
 * create a new AppUIView
 * @param props stateValue.getViewProps()
 */
export function appUIView(props:AppViewProps): React.CElement<AppViewProps, AppUIView>{
    return React.createElement(AppUIView, props)
}
export default class AppUIView extends React.Component<AppViewProps, AppViewState>{
    constructor(props: AppViewProps) {
        super(props);
        this.state = bindUIComponent(this, props).bindHandleAll().setComponentWillUnmount().getState();
    }

    handleClick() {
    }

    render(): React.ReactNode {
        const renderProps = this.props.getRenderProps();
        const navigatorSV = getAppStoreManager().navigatorStore.stateValue;
        return (<>
            {navigatorSV.value && React.createElement(NavigatorView, navigatorSV.getViewProps())}
        </>);
    }
}