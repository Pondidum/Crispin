import { connect } from "react-redux";
import Editor from "./editor";
import { addTag, removeTag } from "../../actions";

const mapStateToProps = (state, ownProps) => {
  return {
    tags: ownProps.toggle.tags
  };
};

const mapDispatchToProps = (dispatch, props) => {
  return {
    addTag: tag => dispatch(addTag(props.toggle.id, tag)),
    removeTag: tag => dispatch(removeTag(props.toggle.id, tag))
  };
};

const connector = connect(
  mapStateToProps,
  mapDispatchToProps
);

export default connector(Editor);
