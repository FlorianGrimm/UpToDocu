import React from "react";
import { getPropertiesChanged } from "dependingState";
import { doSomething } from "./PageAActions";
export function pageAView(props) {
    return React.createElement(PageAView, props);
}
export default class PageAView extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            stateVersion: this.props.getStateVersion()
        };
        this.props.wireStateVersion(this);
        this.handleClick = this.handleClick.bind(this);
        this.handleOnChange = this.handleOnChange.bind(this);
    }
    componentWillUnmount() {
        this.props.unwireStateVersion(this);
    }
    handleClick() {
        doSomething.emitEvent("Hello World!");
    }
    handleOnChange(e) {
        const renderProps = this.props.getRenderProps();
        const renderPropsPC = getPropertiesChanged(renderProps);
        renderPropsPC.setIf("myPropA", e.target.value);
        renderPropsPC.valueChangedIfNeeded("");
    }
    render() {
        const renderProps = this.props.getRenderProps();
        return (React.createElement("div", null,
            React.createElement("h2", null, "PageA"),
            "myPropa:",
            React.createElement("input", { value: renderProps.myPropA, onChange: this.handleOnChange }),
            React.createElement("button", { onClick: this.handleClick }, "doSomething"),
            React.createElement("br", null),
            "myPropB:",
            renderProps.myPropB));
    }
}
