import React, { Component } from 'react'
import { connect } from 'react-redux'
import Toggle from '../Toggle'
import { listAllToggles } from './actions'

const mapStateToProps = (state) => {
    return state.toggles
}

const mapDispatchToProps = dispatch => {
    return {
        listAllToggles: () => dispatch(listAllToggles())
    }
}

class Dashboard extends Component {

    componentDidMount() {
        this.props.listAllToggles();
    }

    render() {
        const toggles = this.props.toggles || []
        return (
            <div className="row">
                <ul>
                    {toggles.map((toggle, index) => (<Toggle key={index} toggle={toggle} />)) }
                </ul>
            </div>
        )
    }
}


export default connect(mapStateToProps, mapDispatchToProps)(Dashboard)
