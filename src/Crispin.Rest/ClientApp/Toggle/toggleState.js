import React, { Component } from 'react'

const prettify = (state) => state === 1 ? 'On' : 'Off';

class ToggleState extends Component {

  constructor() {
    super();
    this.state = { expanded: false }
    this.onDetailsClick = this.onDetailsClick.bind(this)
    this.renderDetails = this.renderDetails.bind(this)
  }

  onDetailsClick(e) {
    e.preventDefault();
    this.setState({
      expanded: !this.state.expanded
    })
  }

  renderDetails() {
    if (!this.state.expanded) {
      return;
    }

    const { state: { anonymous, users, groups } } = this.props;

    const map = (prefix, source) => Object
      .keys(source)
      .map(key => (<li key={key}>{prefix} {key}: {prettify(source[key])}</li>))

    return (<ul className="list-unstyled">
      {map("User", users)}
      {map("Group", groups)}
    </ul>)
  }

  render() {
    const { state: { anonymous, users, groups } } = this.props;
    const specificStates =
      Object.keys(users).length +
      Object.keys(groups).length;

    if (specificStates === 0) {
      return (<p>Default: {prettify(anonymous)}</p>)
    }

    const linkText = this.state.expanded
      ? 'hide details'
      : `and ${specificStates} other states.`

    return (
      <div>
        <p>Default: {prettify(anonymous)} <a href="#" onClick={this.onDetailsClick}>{linkText}</a></p>
        {this.renderDetails()}
      </div>
    )
  }
}

export default ToggleState
