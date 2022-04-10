import {
    bindUIComponent,
    DSUIProps,
    DSUIViewStateBase
} from "dependingState";
import React from "react";
import { getAppStoreManager } from "~/singletonAppStoreManager";
import { pageGraphView } from "~/components/PageGraph/PageGraphView";
import { pageHomeView } from "~/components/PageHome/PageHomeView";
import { NavigatorValue } from "./NavigatorValue";

type NavigatorViewProps = {
} & DSUIProps<NavigatorValue>;

type NavigatorViewState = {
} & DSUIViewStateBase;

export function navigatorView(props: NavigatorViewProps) {
    return React.createElement(NavigatorView, props);
}

export default class NavigatorView extends React.Component<NavigatorViewProps, NavigatorViewState>{
    constructor(props: NavigatorViewProps) {
        super(props);
        this.state = bindUIComponent(this, props).bindHandleAll().setComponentWillUnmount().getState();
    }

    render(): React.ReactNode {
        const renderProps = this.props.getRenderProps();
        // 
        let placeholderPage: any | undefined;
        switch (renderProps.page) {
            case "home":
                placeholderPage = pageHomeView(getAppStoreManager().pageHomeStore.stateValue.getViewProps());
                break;
            case "graph":
                placeholderPage = pageGraphView(getAppStoreManager().pageGraphStore.stateValue.getViewProps());
                break;
            // case "pageA":
            //     placeholderPage = pageAView(getAppStoreManager().pageAStore.stateValue.getViewProps());
            //     break;
            // case "pageB":
            //     placeholderPage = pageBView(getAppStoreManager().pageBStore.stateValue.getViewProps());
            //     break;
            case "pageError":
                placeholderPage = "Error"
            default:
                placeholderPage = "Unknown Page";
                break;
        }
        return (<>
            {placeholderPage}
        </>);
    }
}