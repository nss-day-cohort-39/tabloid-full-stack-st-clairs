import React, { useContext, useEffect, useState } from "react";
import { CommentContext } from "../providers/CommentProvider";
import { Comment } from "./Comment";
import { useParams, Link } from "react-router-dom";
import { PostContext } from "../providers/PostProvider";

export const CommentList = () => {
    const [post, setPost] = useState({})
    const { comments, getCommentsByPostId } = useContext(CommentContext);
    const { getPost } = useContext(PostContext)
    const { id } = useParams()

    useEffect(() => {
        getCommentsByPostId(id);
        getPost(id).then(setPost)
    }, [])

    return (
        <div className="container">
            <div className="row justify-content-center">
                <div className="cards-column">
                    <Link to={`/posts/${id}`}>
                        <button className="btn btn-secondary">Back to Post</button>
                    </Link>
                    <h2>Comments</h2>
                    <br />
                    <h3 className="post_comment">Post: {post.title}</h3>
                    {comments.map((comment) => (
                        <Comment key={comment.id} comment={comment} postId={id} />
                    ))}
                </div>
            </div>
        </div>
    );
};
